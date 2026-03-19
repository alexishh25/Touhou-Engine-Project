# Touhou Engine Project

A technical prototype of a **bullet-hell (danmaku) engine** built in Unity 6 with C#. This project serves as an architecture and gameplay programming experiment, prioritizing scalability, separation of concerns, and code reusability.

This is not a finished game — it is an **extensible technical foundation** that implements the core systems of a danmaku: player control with multiple characters, a shooting system with object pooling, enemies, a full UI built with UI Toolkit, and an async scene loading pipeline.

---

## Architecture

The project splits code into two main layers:

- **`Scripts/`** — Reusable, engine-independent code (managers, controllers, data, utils). Can be extracted into other projects without modifications.
- **`Characteristics/`** — Danmaku-specific logic (player state machine, bullets, enemies). Depends on the base systems in `Scripts/`.

### Persistent Managers (Bootstrap)

The **Bootstrap** system guarantees that global managers are always available, regardless of which scene is opened first in the editor:

```
PerformBootStrap (static)
│   Executes before any Awake() via [RuntimeInitializeOnLoadMethod]
│   Additively loads the "Bootstrap" scene if not already present
│
└── Bootstrap Scene
    ├── BootstrapLoader      → Singleton, DontDestroyOnLoad
    ├── GameManager          → Pause control (TimeScale) and general flow
    ├── InputManager         → Centralizes InputActionAsset, Action Map switching
    ├── SoundManager         → SFX and music playback
    ├── UIManager            → Screen system with UI Toolkit
    └── ButtonManager        → Hover/click SFX registration for UI buttons
```

All managers implement the **Singleton** pattern with `DontDestroyOnLoad`, ensuring a single persistent instance throughout the entire runtime.

### Player State Machine

Player control uses a **custom State Machine** that supports multiple characters with shared and differentiated logic:

```
PlayerBaseState (abstract)
│   Defines: EnterState(), UpdateState(), OnCollisionEnter()
│   Implements shared logic: LogicMoverse(), LogicFocus(), FadeBox()
│
├── PlayerReimuState    → Reimu's concrete state
├── PlayerMarisaState   → Marisa's concrete state
└── PlayerSanaeState    → Sanae's concrete state

PlayerStateManager (MonoBehaviour)
│   Holds references: Rigidbody2D, Animator, InputActions, CharacterData
│   Instantiates all three states in Start()
│   Delegates Update() to the active state: currentState.UpdateState()
│   Exposes SwitchState() for character switching
│   LoadCharacterData() applies ScriptableObject data to the character
```

Each character inherits base movement and focus logic but can override any method to add custom behavior. Per-character configuration (speed, sprites, animations, bullets) is decoupled through `CharacterData` (ScriptableObject).

### Shooting System & Object Pooling

```
BulletPoolManager (Singleton)
│   Pool per prefab type (Dictionary<BulletController, List<BulletController>>)
│   WarmPool() pre-instantiates bullets at startup
│   RequestBullet() returns an inactive bullet or creates a new one if the pool is exhausted
│
BulletManager (per entity)
│   Each shooter (player/enemy) owns its own BulletManager
│   Controls cooldown, speed, and shoot point
│   Requests bullets from BulletPoolManager
│
BulletController
│   Controls movement (velocity * deltaTime) and lifecycle (MAX_LIFE_TIME)
│   Deactivates instead of destroying to return to the pool
│
EnemyShooter
    Shoots towards the player using the same pooling system
    Calculates direction: (player.position - transform.position).normalized
```

### UI System (UI Toolkit)

The UI uses **UI Toolkit** (UXML/USS) with a swappable screen system:

```
ScreenLogic (abstract MonoBehaviour)
│   Base template for each screen
│   Defines: DefinirElementos(), ButtonActionAlterSusYUnsuscribe(), LoadData()
│   Initialize() / Dispose() for lifecycle management
│
├── MenuScript           → Main menu (Game Start, Extra, Practice, Quit...)
├── SelectorScript       → Character selector with navigation and transitions
├── LoadingScript        → Loading screen with VFX
├── HUDScript            → Gameplay HUD
└── PauseUIScript        → Pause menu (Continue, Quit and Restart, Return to Game)

MenuUIController (per scene)
│   Defines which screens (ScreenEntry[]) are available in the scene
│   Registers screens with UIManager on startup
│
UIManager (persistent Singleton)
    Controls screen switching: ChangeScreen(ScreenType)
    Handles interpolated scene loading: InterpolateScreenLoad()
    Manages the active UIDocument
```

---

## Execution Flow

```
1. STARTUP
   └── PerformBootStrap.Execute()
       └── Loads "Bootstrap" scene (additive)
           └── All Managers initialize (DontDestroyOnLoad)

2. MAIN MENU (Scene: UI/Menu)
   └── MenuUIController registers screens with UIManager
       └── UIManager displays MainMenu (MenuScript)
           └── Player presses "Game Start"
               └── UIManager switches to SelectCharacter (SelectorScript)

3. CHARACTER SELECTION
   └── SelectorScript loads data from SelectorCharacterData
       └── Player navigates between characters (Navigate input)
           └── On confirm (Submit):
               ├── InputManager switches Action Map: "UI" → "Player"
               └── UIManager.InterpolateScreenLoad("Gameplay")

4. SCENE LOADING
   └── "LoadingScreen" loads (additive)
       └── "Gameplay" loads (additive, async)
           └── On completion:
               ├── LoadingScreen unloads
               ├── Previous scene unloads
               ├── InputManager activates "Player" Action Map
               └── UIManager disables the UIDocument

5. GAMEPLAY (Scene: Gameplay)
   └── PlayerStateManager initializes character state
       ├── Loads CharacterData → Applies stats, animator, bullet sprite
       ├── Active state runs UpdateState() every frame
       │   ├── LogicMoverse() → Input + Rigidbody2D + Animation Blend
       │   └── LogicFocus() → Reduces speed + Fades hitbox
       ├── BulletManager fires bullets from the pool (Input: Shoot)
       └── EnemyShooter fires towards the player from the pool
```

---

## Folder Structure

```
Assets/TouhouEngine/
│
├── Characteristics/                  # Danmaku-specific logic
│   ├── Bullets/
│   │   ├── SimpleBullet/             # BulletController, BulletManager, BulletPoolManager
│   │   └── RadialShotSettings.cs     # Radial pattern configuration
│   ├── Enemy/
│   │   └── Scripts/                  # EnemyShooter, EnemyBulletManager
│   └── Player/
│       ├── Player Controller.cs      # Original controller (pre-refactor)
│       └── Scripts/
│           ├── Player Base State.cs      # Abstract base class
│           ├── Player State Manager.cs   # State machine brain
│           ├── Player Reimu State.cs     # Concrete state
│           ├── Player Marisa State.cs    # Concrete state
│           ├── Player Sanae State.cs     # Concrete state
│           ├── Core/Hitbox.cs
│           └── Data/CharacterData.cs     # Character ScriptableObject
│
├── Scripts/                          # Reusable code
│   ├── Controllers/                  # AnimationUI, Audio, Editor, Enemy, PlayerAnimation
│   ├── Core/                         # BootstrapLoader
│   ├── Data/                         # AnimationData, BulletPatternData, SelectorCharacterData
│   ├── Managers/                     # GameManager, InputManager, SoundManager, UIManager
│   ├── Services/                     # (Reserved)
│   ├── Systems/                      # (Reserved)
│   └── Utils/                        # BoxColliderFit
│
├── UI/                               # UI Toolkit
│   ├── Components/CustomElements/    # LeafParticleElement, TransitionElement
│   ├── Controllers/                  # MenuUIController
│   ├── HUD/                          # HUDScript, PauseUIScript
│   └── Menu/                         # MenuScript, SelectorScript, LoadingScript, ScreenLogic
│
├── ScriptableObjects/                # Configurable data assets
│   ├── Characters/                   # CharacterData per character
│   ├── Bullets/                      # Bullet configuration
│   ├── Patterns/                     # Shooting patterns
│   └── Audio/                        # Audio configuration
│
├── Shaders/                          # Shader Graph (ScrollingBackground, Leaf)
├── Prefabs/                          # Player, Enemies, Bullets, Effects, VFX
├── Scenes/                           # Bootstrap, Gameplay
├── Art/                              # Visual assets
├── Sounds/                           # Audio assets
└── Fonts/                            # Fonts
```

---

## Technologies

- **Unity 6** (URP)
- **C#**
- **Unity Input System** — Multiple Action Maps (Player, UI, Gameplay)
- **UI Toolkit** — UXML/USS for the entire interface
- **Shader Graph** — Custom visual shaders
- **ScriptableObjects** — Configuration data decoupling
- **Object Pooling** — Instantiation optimization for the bullet system

---

## Project Status

Actively evolving project. Core systems are implemented and functional. Areas prepared for expansion: `Services/`, `Systems/`, advanced bullet patterns (radial shots), and per-character differentiated logic.
