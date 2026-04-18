using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 10f;
    private float lifeTimer = 0f;

    public Vector2 Velocity;
    public SpriteRenderer _rendererbullet;
    private Sprite defaultSprite;

    private void Awake()
    {
        _rendererbullet = GetComponent<SpriteRenderer>();
        if (_rendererbullet != null)
            defaultSprite = _rendererbullet.sprite;
    }

    private void OnEnable()
    {
        lifeTimer = 0f;
        if (_rendererbullet != null && defaultSprite != null)
            _rendererbullet.sprite = defaultSprite;
    }


    private void Movement()
    {
        transform.position += (Vector3)Velocity * Time.deltaTime;
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= MAX_LIFE_TIME) Destroy();
    }

    public void ChangeSprite(Sprite newSprite)
    {
        if (_rendererbullet != null)
        {
            _rendererbullet.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found in BulletController to change the sprite.");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {

        }
    }
}
