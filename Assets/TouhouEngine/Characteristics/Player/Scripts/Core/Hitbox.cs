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
        Efectos();
    }

    private void Efectos()
    {
        hitbox.Rotate(0, 0, velocidad * Time.deltaTime);
    }
}
