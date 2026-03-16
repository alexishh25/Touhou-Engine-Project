using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth = 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
