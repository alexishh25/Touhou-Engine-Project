using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStats stats;
    private int currentHealth;

    public event Action OnEnemyDeath;
    public event Action TakeDamaged;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        if (stats != null)
            currentHealth = stats.health;
    }

    public void TakeDamage(int damage)
    {
        TakeDamaged?.Invoke();
        int finalDamage = Mathf.Max(1, damage - (stats != null ? stats.defense : 0));
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}
