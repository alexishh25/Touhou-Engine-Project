using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public struct Transition_Screen
{
    [CreateProperty] public PlayableDirector transition;
    [CreateProperty] public bool Reverse;
}
[CreateAssetMenu(fileName = "New TransitionScreen", menuName = "Animation / TransitionScreenData")]
public class TransitionScreenData : ScriptableObject
{
    [SerializeField] Transition_Screen Screen1, Screen2;

}
