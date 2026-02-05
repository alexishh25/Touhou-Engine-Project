using UnityEngine;
using UnityEngine.InputSystem;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float shotCooldown;
    [SerializeField] private float bulletSpeed;

    private float shotTimerCooldown = 0f;
    public InputAction actionShot;

    private void Awake()
    {
        actionShot = InputSystem.actions.FindAction("Shoot");
    }


    private void Shot(Vector2 origin, Vector2 velocity)
    {
        BulletController bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;

        bullet.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (shotTimerCooldown > 0) shotTimerCooldown -= Time.deltaTime;

        if (shotTimerCooldown <= 0f && actionShot.ReadValue<float>() > 0.5f)
        {
            Shot(transform.position, transform.up * bulletSpeed);
            shotTimerCooldown = shotCooldown;
        }
    }
}
