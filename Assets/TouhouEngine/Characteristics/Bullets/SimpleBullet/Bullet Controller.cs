using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 10f;
    private float lifeTimer = 0f;

    public Transform puntoDisparo;
    public Vector2 Velocity;
    public SpriteRenderer _rendererbullet;
    public AudioSource audioSource;

    private void Awake()
    {
        _rendererbullet = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Movement()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = puntoDisparo.forward * Velocity;

        transform.position += (Vector3)Velocity * Time.deltaTime;
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= MAX_LIFE_TIME) Destroy();
    }

    public void ChangeSprite(Sprite newSprite)
    {
        if (_rendererbullet != null)
        {
            _rendererbullet.sprite = newSprite;
            Debug.Log("Sprite de bala cambiado a: " + newSprite.name);
        }
        else
        {
            Debug.LogWarning("No se encontr� SpriteRenderer en el BulletController para cambiar el sprite.");
        }
    }

    private void Destroy()
    {
        lifeTimer = 0f;
        gameObject.SetActive(false);  
    }
    private void Update()
    {
        Movement();
    }
}
