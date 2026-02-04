using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Animación", menuName = "Animation / AnimationPlayerData")]
public class AnimationData : ScriptableObject
{
    [Header("Campos de Animacion")]
    public string animationName;
    public float frameloop;
}
