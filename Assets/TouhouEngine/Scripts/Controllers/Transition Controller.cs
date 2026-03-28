using UnityEngine;
using UnityEngine.Playables;

public class TransitionController : MonoBehaviour
{
    public static TransitionController Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    [Header("Timelines")]
    [SerializeField] public PlayableDirector exitTransition;
    [SerializeField] public PlayableDirector enterTransition;
}
