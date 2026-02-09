using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo personaje", menuName = "TouhouEngine/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Identidad")]
    public string characterName;

    [Header("Stats")]
    public float basemoveSpeed = 10f;
    public float focusSpeed = 2.3f;

    [Header("Animaciones")]
    public AnimatorOverrideController animatorController;

    [Header("Bullets")]
    public Sprite bulletSprite;
    public AudioClip bulletSound;
    public GameObject bulletFocusPrefab;
}
