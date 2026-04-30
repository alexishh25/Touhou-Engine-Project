using UnityEngine;

public class InfiniteStairs : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] public Camera _camera;
    [SerializeField] public GameObject[] stairs;

    [Header("Configuración de Movimiento")]
    public float downspeed = 2.0f;
    public float backspeed = 2.0f;

    [Header("Configuración de la Escalera")]
    public Vector3 initialPosition = new Vector3(0, 0, 0);
    public float stepHeight = 1.0f; // Separación vertical (Y) entre escalones

    public void RepositionStair(Transform stair_transform)
    {
        float highestY = float.MinValue;

        // Buscamos cuál es el escalón que está más arriba en este momento
        for (int i = 0; i < stairs.Length; i++)
        {
            if (stairs[i] != null && stairs[i].transform.localPosition.y > highestY)
            {
                highestY = stairs[i].transform.localPosition.y;
            }
        }

        // Lo colocamos justo por encima del más alto, manteniendo su X y Z intactos
        Vector3 newLocalPosition = stair_transform.localPosition;
        newLocalPosition.y = highestY + stepHeight;
        
        stair_transform.localPosition = newLocalPosition;
        // Debug.Log($"Stair repositioned to local Y: {newLocalPosition.y}");
    }

    private void Update()
    {
        var cameraBottomY = _camera.WorldToViewportPoint(transform.position).y;
        for (int i = 0; i < stairs.Length; i++)
        {
            if (stairs[i] == null) continue;

            stairs[i].transform.Translate(Vector3.down * downspeed * Time.deltaTime, Space.World);
            stairs[i].transform.Translate(Vector3.back * backspeed * Time.deltaTime, Space.World);
        }
    }
}
