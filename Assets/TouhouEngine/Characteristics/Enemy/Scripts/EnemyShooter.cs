using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private float shotCooldown = 1.5f;
    [SerializeField] private BulletController bulletPrefab;

    [Header("Sonidos")]
    [SerializeField] private AudioClip bulletSfx;
    [SerializeField] float volumen = 0.4f;

    private float timer = 0f;
    private Transform player;

    private float bulletSpeed = 5f;
    private float lifeDuration = 0f;
    private BulletSpawnInstruction bulletInstruction;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void InitializeFromTimeline(SpawnInstruction instruction)
    {
        this.lifeDuration = instruction.duration;
        this.bulletInstruction = instruction.bullet;
        this.bulletSpeed = instruction.bullet.speed;

        if (lifeDuration > 0)
        {
            Die(lifeDuration);
        }
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
        Vector2 finalvelocity = direction * bulletSpeed;

        BulletController bullet = BulletPoolManager.Instance.RequestBullet(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.Velocity = finalvelocity;
        bullet.gameObject.SetActive(true);
        
        SoundManager.Instance.PlaySFX(bulletSfx, volumen);
    }

    private void Die(float duration)
    {
        Destroy(gameObject, duration);
    }
}
