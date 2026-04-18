using UnityEngine;

// ¡Este script ahora es Ciego y Obediente! Actuador Puro (Arma).
public class EnemyShooter : MonoBehaviour
{
    [Header("Weapon Config")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private BulletController bulletPrefab;

    [Header("Sonidos")]
    [SerializeField] private AudioClip bulletSfx;
    [SerializeField] private float volumen = 0.4f;

    private Transform player;

    private void Start()
    {
        // Solo necesita ubicarse con su objetivo
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public void FirePattern()
    {
        if (player == null || bulletPrefab == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 finalVelocity = direction * bulletSpeed;

        BulletController bullet = BulletPoolManager.Instance.RequestBullet(bulletPrefab);
        
        // Respetamos arquitectura Pool
        bullet.transform.position = transform.position;
        bullet.Velocity = finalVelocity; 
        bullet.gameObject.SetActive(true);
        
        SoundManager.Instance.PlaySFX(bulletSfx, volumen);
    }
}
