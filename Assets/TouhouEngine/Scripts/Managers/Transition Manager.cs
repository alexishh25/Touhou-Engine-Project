using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Playables;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance {  get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
    }

    // onMiddle  : called after exit anim → instantiates new screen
    // onFinish  : called after new screen is added → removes old screen
    // onComplete: called after enter anim finishes → releases transition lock
    public void PlayTransition(TransitionScreenData data, Action onMiddle = null, Action onFinish = null, Action onComplete = null)
    {
        // Bug #4 fix: guard against null transition asset so we don't stomp a running director
        if (data.exitTransition.transition != null)
        {
            var exitDirector = TransitionController.Instance.exitTransition;
            exitDirector.playableAsset = data.exitTransition.transition;
            TimelineManager.Instance.PlayForward(exitDirector, data.exitTransition.Reverse);
        }

        TransitionRoutine(data, onMiddle, onFinish, onComplete).Forget();
    }

    private async UniTaskVoid TransitionRoutine(TransitionScreenData data, Action onMiddle, Action onFinish, Action onComplete)
    {
        // Bug #1 fix: wait for the exit animation to fully complete before swapping screens.
        var exitDirector = TransitionController.Instance.exitTransition;
        if (data.exitTransition.transition != null && exitDirector != null)
            await UniTask.WaitForSeconds((float)exitDirector.duration);

        // Designer-configurable pause between exit finishing and enter starting
        if (data.interval > 0)
            await UniTask.WaitForSeconds(data.interval);

        // 1) Add the new screen to the DOM
        onMiddle?.Invoke();

        // 2) Remove the old screen from the DOM BEFORE setting up the enter animation.
        //
        // ROOT CAUSE OF THE FLICKER: UITVisualElementTrack.CreateTrackMixer() calls
        // rootVisualElement.Query().Name("...") at the moment playableAsset is assigned.
        // If the old screen is still in the DOM, the query finds its elements first
        // and the enter animation binds to the WRONG elements. The new screen then
        // appears for 1+ frames at its default UXML positions before anything corrects it.
        onFinish?.Invoke();

        // 3) NOW set up and play the enter animation — only the new screen is in the DOM,
        //    so CreateTrackMixer() will bind exclusively to the correct elements.
        if (data.enterTransition.transition != null)
        {
            var enterDirector = TransitionController.Instance.enterTransition;
            enterDirector.playableAsset = data.enterTransition.transition;
            TimelineManager.Instance.PlayForward(enterDirector, data.enterTransition.Reverse);

            // Wait for the enter animation to fully play out before releasing the lock
            await UniTask.WaitForSeconds((float)enterDirector.duration);
        }

        // 4) Release the transition lock only after the enter animation has finished.
        //    This prevents triggering a new transition while enter is still playing.
        onComplete?.Invoke();
    }


}
