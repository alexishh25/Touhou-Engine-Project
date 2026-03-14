using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "TouhouEngine/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Identity")]
    public string characterName;

    [Header("Stats")]
    public float basemoveSpeed = 10f;
    public float focusSpeed = 2.3f;

    [Header("Animations")]
    public AnimatorOverrideController animatorController;

    [Header("Bullets")]
    public Sprite bulletSprite;
    public AudioClip bulletSound;
    public GameObject bulletFocusPrefab;
}
