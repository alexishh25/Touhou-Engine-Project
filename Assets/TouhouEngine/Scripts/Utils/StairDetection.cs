using UnityEngine;

public class StairDetection : MonoBehaviour
{
    public InfiniteStairs stairs;

    // Cambiado a 3D y usando Trigger en lugar de Collision
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("StairDetection: Trigger entered by " + other.gameObject.name);
        // Validamos que chocó con el EndCollider
        if (other.gameObject.CompareTag("Collider"))
        {
            stairs.RepositionStair(this.transform);
        }
    }
}
