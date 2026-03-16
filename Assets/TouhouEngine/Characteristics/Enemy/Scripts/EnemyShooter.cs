using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private float shotCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private BulletController bulletPrefab;

    [Header("Sonidos")]
    [SerializeField] private AudioClip bulletSfx;
    [SerializeField] float volumen = 0.4f;

    private float timer = 0f;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ShootAtPlayer();
            timer = shotCooldown;
        }

    }

    private void ShootAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 velocity = direction * bulletSpeed;

        BulletController bullet = BulletPoolManager.Instance.RequestBullet(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.Velocity = velocity;
        bullet.gameObject.SetActive(true);
        
        SoundManager.Instance.PlaySFX(bulletSfx, volumen);
    }
}
