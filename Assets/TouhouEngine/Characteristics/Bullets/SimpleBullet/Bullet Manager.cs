using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Es el encargado de manejar el disparo de las balas. Lee la entrada del jugador para determinar cu�ndo disparar, y luego solicita una bala del BulletPool para activarla y darle la velocidad adecuada.
/// Tambi�n puede cambiar el sprite de las balas si es necesario.
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
