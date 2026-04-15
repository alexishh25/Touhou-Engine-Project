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
        var exitDirector = TransitionController.Instance.exitTransition;
        if (data.exitTransition.transition != null && exitDirector != null)
            await UniTask.WaitForSeconds((float)exitDirector.duration);

        //if (data.interval > 0)
        //    await UniTask.WaitForSeconds(data.interval);

        onMiddle?.Invoke();

        if (data.enterTransition.transition != null)
        {
            var enterDirector = TransitionController.Instance.enterTransition;
            enterDirector.playableAsset = data.enterTransition.transition;
            TimelineManager.Instance.PlayForward(enterDirector, data.enterTransition.Reverse);

            await UniTask.WaitForSeconds((float)enterDirector.duration);
        }

        onFinish?.Invoke();

        onComplete?.Invoke();
    }


}
