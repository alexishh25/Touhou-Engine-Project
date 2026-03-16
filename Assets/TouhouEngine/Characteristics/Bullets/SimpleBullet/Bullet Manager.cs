using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles bullet firing for a single entity (player or enemy).
/// Not a singleton - each shooter owns its own instance.
/// </summary>
public class BulletManager : MonoBehaviour
{
    [SerializeField] private float shotCooldown;
    [SerializeField] private float bulletSpeed;
    [SerializeField] public AudioClip bullet_sfx;
    [SerializeField] private Transform shootPoint; // Child GameObject placed at the desired shoot origin
    [SerializeField] private BulletController bulletPrefab;

    private float shotTimerCooldown = 0f;
    private InputAction actionShot;
    private Sprite bulletsprite;

    private void Awake() { }

    private void Start()
    {
        actionShot = GameManager.Instance.inputActions
            .FindActionMap("Player")
            .FindAction("Shoot");

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
    public static void RadialShot(Vector2 origin, Vector2 aimDirection, RadialShotSettings settings)
    {

    }
    private void Shot()
    {
        // Use shootPoint if assigned, otherwise fall back to this transform
        Vector2 origin = shootPoint != null ? shootPoint.position : transform.position;
        Vector2 velocity = transform.up * bulletSpeed;

        BulletController bullet = BulletPoolManager.Instance.RequestBullet(bulletPrefab);
        bullet.transform.position = origin;
        bullet.Velocity = velocity;

        //if (bulletsprite != null)
        //    bullet.ChangeSprite(bulletsprite);

        bullet.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX(bullet_sfx);
    }

    private void Update()
    {
        if (shotTimerCooldown > 0) shotTimerCooldown -= Time.deltaTime;

        if (shotTimerCooldown <= 0f && actionShot != null && actionShot.ReadValue<float>() > 0.5f)
        {
            Shot();
            shotTimerCooldown = shotCooldown;
        }
    }
}
