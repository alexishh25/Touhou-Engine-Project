using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public struct Transition_Screen
{
    [CreateProperty] public TimelineAsset transition;
    [CreateProperty] public bool Reverse;
}
[CreateAssetMenu(fileName = "New TransitionScreen", menuName = "Animation / TransitionScreenData")]
public class TransitionScreenData : ScriptableObject
{
    [Header("Salida")]
    [SerializeField] public Transition_Screen exitTransition;

    [Header("Entrada")]
    [SerializeField] public Transition_Screen enterTransition;

    [Header("Timing")]
    [SerializeField][Range(0f, 1f)] public float interval; 
    [SerializeField] bool waitForExit; 

    [Header("Input")]
    [SerializeField] bool blockInputDuring;
}
