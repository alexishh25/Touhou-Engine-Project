using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] public float velocidad = 1f;
    private Transform hitbox;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        hitbox = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Effects();
    }

    private void Effects()
    {
        hitbox.Rotate(0, 0, velocidad * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            PlayerStateManager.Instance.currentStats.OnCollisionEnter();
        }
    }
}
