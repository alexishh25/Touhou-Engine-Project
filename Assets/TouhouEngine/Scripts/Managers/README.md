# Managers — TransitionManager & TimelineManager

This document covers the two runtime managers responsible for all UI screen transitions in the Touhou Engine. Both are persistent singletons (`DontDestroyOnLoad`) that live on the **UIManager GameObject** in the main scene.

---

## TimelineManager

**File:** `Timeline Manager.cs`

A thin wrapper around Unity's `PlayableDirector` that adds **manual reverse playback** support, which the built-in `PlayableDirector` API does not offer out of the box.

### Why it exists

Unity's `PlayableDirector` can only play a timeline forward (`Play()`) or be scrubbed to any time via `time`. There is no native "play backwards at real-time speed" option. `TimelineManager` implements this by registering directors into a `reversingDirectors` list and manually decrementing their `time` each `Update` frame.

### Public API

```csharp
TimelineManager.Instance.PlayForward(PlayableDirector timeline, bool reverse = false);
```

| Parameter | Description |
|-----------|-------------|
| `timeline` | The `PlayableDirector` to control. Must not be null. |
| `reverse`  | `false` = play from time 0 forward (normal). `true` = play from the end of the timeline backwards at real-time speed. |

#### Forward mode (`reverse = false`)
1. Removes the director from the reverse list if it was previously reversing.
2. Resets time to `0` and calls `Evaluate()` immediately — this forces the PlayableGraph to render frame 0 of the animation in the same frame the call is made, preventing a one-frame flash at the wrong position.
3. Calls `Play()` to let Unity advance the timeline normally.
4. Ensures the root playable speed is set to `1` (protects against a stalled graph from a previous operation).

#### Reverse mode (`reverse = true`)
1. Registers the director in `reversingDirectors` if not already there.  
2. Calls `Pause()` so Unity stops its automatic forward advancement.
3. Sets `time` to `duration` and calls `Evaluate()` — snaps to the last frame immediately.
4. From here, `Update()` → `ProcessReversingDirectors()` takes over each frame, decrementing `time` by `Time.deltaTime` until it reaches `0`, then removes the director from the list.

### Internal: `ProcessReversingDirectors()` (called in `Update`)

Iterates the reverse list back-to-front (safe for mid-loop removal):
- Skips and removes any `null` director (destroyed at runtime).
- Subtracts `Time.deltaTime` from the director's `time`.
- Calls `Evaluate()` to push the visual update for that frame.
- When `time <= 0`: clamps to `0`, does a final `Evaluate()`, and removes the director — the animation has finished reversing.

---

## TransitionManager

**File:** `Transition Manager.cs`

Orchestrates the full lifecycle of a UI screen transition: **exit animation → swap → enter animation**. It works in lockstep with `UIManager` (which owns the UXML DOM) and `TimelineManager` (which drives the `PlayableDirector`s).

### Dependencies

| Component | Role |
|-----------|------|
| `TransitionController` | Holds references to the two shared `PlayableDirector`s (`exitTransition`, `enterTransition`). |
| `TimelineManager` | Starts/reverses each director. |
| `TransitionScreenData` | ScriptableObject configured per-transition (see below). |
| `UIManager` | Provides the three lifecycle callbacks. |

### `TransitionScreenData` (ScriptableObject)

Create via **Assets → Animation → TransitionScreenData**.

| Field | Type | Description |
|-------|------|-------------|
| `exitTransition.transition` | `TimelineAsset` | Timeline to play while the current screen exits. |
| `exitTransition.Reverse` | `bool` | If true, the exit timeline plays backwards. |
| `enterTransition.transition` | `TimelineAsset` | Timeline to play while the new screen enters. |
| `enterTransition.Reverse` | `bool` | If true, the enter timeline plays backwards. |
| `interval` | `float` (0–3 s) | Extra pause inserted between the exit finishing and the enter starting. |

### Public API

```csharp
TransitionManager.Instance.PlayTransition(
    TransitionScreenData data,
    Action onMiddle   = null,   // called after exit → instantiates new screen
    Action onFinish   = null,   // called after new screen is added → removes old screen
    Action onComplete = null    // called after enter finishes → releases transition lock
);
```

`UIManager.ChangeScreen` is the only caller in normal use. It passes:
- **`onMiddle`** → `PerformChange()` — instantiates the new UXML screen and initialises its `ScreenLogic`.
- **`onFinish`** → `CleanupOldScreens()` — removes the previous screen's `VisualElement` children from the root.
- **`onComplete`** → `() => _isTransitioning = false` — releases the re-entrancy lock so new transitions can start.

### Execution order (inside `TransitionRoutine`)

```
[Same frame as PlayTransition()]
  • Exit timeline starts playing (or skipped if exitTransition.transition is null)

[Async — on a UniTask]
  1. await exit animation duration        → waits for exit to fully complete
  2. await interval                       → optional designer pause (if > 0)
  3. onMiddle()                           → new screen added to the UI DOM
  4. onFinish()                           → OLD screen removed from the UI DOM  ← critical order
  5. Enter timeline starts playing        → PlayableGraph binds ONLY to new screen elements
  6. await enter animation duration       → waits for enter to fully complete
  7. onComplete()                         → transition lock released
```

### ⚠️ Critical ordering: why `onFinish` runs before the enter animation

The `UITTimeline` package (`UITVisualElementTrack.CreateTrackMixer`) queries all `VisualElement` targets by name/class **at the exact moment `playableAsset` is assigned** to a `PlayableDirector`. If the old screen is still in the DOM at that point, the query can match elements from the wrong screen (e.g., a `#QuitButton` from the old screen instead of the new one). This causes the new screen to flash at its default UXML layout positions for one or more frames — the visible "flicker" on transitions.

**The fix:** always remove the old screen (`onFinish`) before assigning the enter `playableAsset`.

### Re-entrancy guard

`UIManager` sets `_isTransitioning = true` before calling `PlayTransition` and only resets it in `onComplete` (after the enter animation finishes). Any call to `UIManager.ChangeScreen` while a transition is running is silently dropped, preventing race conditions from rapid button presses.

---

## Typical call flow (example: Settings → Menu)

```
SettingsScript.OnQuitButtonClicked()
  └─ UIManager.ChangeScreen(MainMenu, menuTransitionData)
       │  _isTransitioning = true
       └─ TransitionManager.PlayTransition(data, PerformChange, CleanupOldScreens, ReleaseLock)
            │  exitDirector.playableAsset = data.exitTransition   // SettingsTransition exit
            │  TimelineManager.PlayForward(exitDirector)          // slides settings elements out
            └─ TransitionRoutine() [async]:
                 await exit.duration                              // wait for slide-out to finish
                 PerformChange()                                  // Menu UXML added to root
                 CleanupOldScreens()                             // Settings UXML removed
                 enterDirector.playableAsset = data.enterTransition
                 TimelineManager.PlayForward(enterDirector)       // slides menu elements in
                 await enter.duration                             // wait for slide-in to finish
                 _isTransitioning = false                         // lock released
```
