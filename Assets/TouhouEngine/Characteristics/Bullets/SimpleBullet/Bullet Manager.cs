using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles bullet firing. Reads player input to determine when to shoot, then requests a bullet from the BulletPool to activate it and set the appropriate velocity.
/// Can also change the bullet sprite if needed.
/// </summary>
public class BulletManager : MonoBehaviour
{
    [SerializeField] private float shotCooldown;
    [SerializeField] private float bulletSpeed;
    [SerializeField] public AudioClip bullet_sfx;

    private float shotTimerCooldown = 0f;
    private InputAction actionShot;

    private Sprite bulletsprite;
    private void Awake()
    {
        actionShot = GameManager.Instance.inputActions
            .FindActionMap("Player")
            .FindAction("Shoot");
    }

    private void OnEnable()
    {
        actionShot?.Enable();
    }

    private void OnDisable()
    {
        actionShot?.Disable();
    }

    public void SetAmmoSprite(Sprite newSprite)
    {
        bulletsprite = newSprite;
    }

    private void Shot(Vector2 origin, Vector2 velocity)
    {
        BulletController bullet = BulletPool.Instance.RequestBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;

        if (bulletsprite != null)
        {
            bullet.ChangeSprite(bulletsprite);
        }

        bullet.gameObject.SetActive(true);

        SoundManager.Instance.PlaySFX(bullet_sfx);
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
