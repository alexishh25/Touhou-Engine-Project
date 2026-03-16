using UnityEngine;

[CreateAssetMenu(fileName = "BulletPatternData", menuName = "TouhouEngine/BulletPatternData")]
public class BulletPatternData : ScriptableObject
{
    public int Repitions;
    public RadialShotSettings[] radialShotSettings;
}
