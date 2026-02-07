using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Es el encargado de manejar el disparo de las balas. Lee la entrada del jugador para determinar cuándo disparar, y luego solicita una bala del BulletPool para activarla y darle la velocidad adecuada.
/// También puede cambiar el sprite de las balas si es necesario.
/// </summary>
public class BulletManager : MonoBehaviour
{
    [SerializeField] private float shotCooldown;
    [SerializeField] private float bulletSpeed;

    private float shotTimerCooldown = 0f;
    public InputAction actionShot;

    private Sprite bulletsprite;
    private void Awake()
    {
        actionShot = InputSystem.actions.FindAction("Shoot");
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
