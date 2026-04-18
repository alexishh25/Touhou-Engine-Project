using UnityEngine;
using System;

public class EnemyStats : MonoBehaviour
{
    [Serializable]
    public enum BulletType
    {
        Straight,
        SineWave,
        Sinusoidal
    }

    public int health = 100;
    public int attack = 10;
    public int defense = 5;

    public float speed = 5f;
    public float attackSpeed = 1f;
    public float defenseSpeed = 1f;

    public bool stream = false;
    public BulletType bulletType = BulletType.Straight;
}
