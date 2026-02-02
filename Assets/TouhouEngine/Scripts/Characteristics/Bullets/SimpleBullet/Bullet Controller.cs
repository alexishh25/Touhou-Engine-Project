using UnityEngine;

public class BulletController : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 10f;
    private float lifeTimer = 0f;
    public Vector2 Velocity;

    private void Movement()
    {
        transform.position += (Vector3)Velocity * Time.deltaTime;
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= MAX_LIFE_TIME) Destroy();
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
