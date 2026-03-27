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

    public void PlayTransition(TransitionScreenData data, Action onMiddle = null)
    {
        var director = TimelineController.Instance.director;
        director.playableAsset = data.exitTransition.transition;
        TimelineManager.Instance.PlayForward(director, data.exitTransition.Reverse);

        TransitionRoutine(director, data, onMiddle).Forget();
    }

    private async UniTaskVoid TransitionRoutine(PlayableDirector director, TransitionScreenData data, Action onMiddle)
    {
        await UniTask.WaitForSeconds((float)director.duration + data.interval);

        onMiddle?.Invoke();

        director.playableAsset = data.enterTransition.transition;
        TimelineManager.Instance.PlayForward(director, data.enterTransition.Reverse);
    }


}
