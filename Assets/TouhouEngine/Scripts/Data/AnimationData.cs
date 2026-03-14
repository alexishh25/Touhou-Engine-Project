using UnityEngine;

[CreateAssetMenu(fileName = "New Animation", menuName = "Animation / AnimationPlayerData")]
public class AnimationData : ScriptableObject
{
    [Header("Animation Fields")]
    public string animationName;
    public float frameloop;
}
