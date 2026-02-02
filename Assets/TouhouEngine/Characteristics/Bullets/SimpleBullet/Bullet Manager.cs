using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float shotCooldown;
    [SerializeField] private float bulletSpeed;

    private float shotTimerCooldown = 0f;
    private void Shot(Vector2 origin, Vector2 velocity)
    {
        BulletController bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;

        bullet.gameObject.SetActive(true);
    }

    private void Update()
    {
        shotTimerCooldown -= Time.deltaTime;

        if (shotTimerCooldown <= 0f)
        {
            Shot(transform.position, transform.right * bulletSpeed);
            shotTimerCooldown += shotCooldown;
        }
    }
}
