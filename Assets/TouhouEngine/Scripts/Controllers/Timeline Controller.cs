using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController Instance { get; private set; }
    public PlayableDirector director;

    [Header("Variables")]
    [SerializeField] public bool isPlaying => director.state == PlayState.Playing;
    [SerializeField] public bool isReversing { get; private set; }
    [SerializeField] public float Progress => (float)(director.time / director.duration);

    public void Awake()
    {
        Instance = this;
        director = GetComponent<PlayableDirector>();
    }

    public void Start()
    {
        director.playableAsset = null;
    }

    public void SetSpeed(float speed)
    {
        if (director.playableGraph.IsValid())   
            director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
    }

    public void ReproducirTransicionUI(TransitionScreenData data) => TransitionManager.Instance.PlayTransition(data);

}
