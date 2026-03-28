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
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayTransition(TransitionScreenData data, Action onMiddle = null, Action onFinish = null)
    {
        var exitTransition = TransitionController.Instance.exitTransition;
        exitTransition.playableAsset = data.exitTransition.transition;
        TimelineManager.Instance.PlayForward(exitTransition, data.exitTransition.Reverse);

        TransitionRoutine(data, onMiddle, onFinish).Forget();
    }

    private async UniTaskVoid TransitionRoutine(TransitionScreenData data, Action onMiddle, Action onFinish)
    {
        onMiddle?.Invoke();

        var enterTransition = TransitionController.Instance.enterTransition;
        enterTransition.playableAsset = data.enterTransition.transition;
        TimelineManager.Instance.PlayForward(enterTransition, data.enterTransition.Reverse);

        await UniTask.WaitForSeconds(data.interval);

        // Calculate how long to wait until BOTH transitions are completely done
        var exitTransition = TransitionController.Instance.exitTransition;
        float remainingExit = Mathf.Max(0, (float)exitTransition.duration - data.interval);
        float enterDuration = (float)enterTransition.duration;
        float waitTime = Mathf.Max(remainingExit, enterDuration);

        await UniTask.WaitForSeconds(waitTime);

        onFinish?.Invoke();
    }


}
