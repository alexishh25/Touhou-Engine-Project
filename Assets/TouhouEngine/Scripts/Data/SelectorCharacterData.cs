using Unity.Properties;
using UnityEngine;
using System;

[Serializable]
public struct SelectCharacter
{
    [CreateProperty] public string Name;
    [CreateProperty] public Sprite Sprite;
    [CreateProperty] public Sprite Sprite_shadow;
    [CreateProperty] public string subdesc;
    [CreateProperty] public string desc;
}

[CreateAssetMenu(fileName = "SelectorCharacterData", menuName = "TouhouEngine/SelectorCharacterData")]
public class SelectorCharacterData : ScriptableObject
{
    
    [SerializeField] public SelectCharacter[] characterDataArray;
}
