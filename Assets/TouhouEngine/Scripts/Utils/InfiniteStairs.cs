using UnityEngine;

public class InfiniteStairs : MonoBehaviour
{
    [SerializeField] public Camera _camera;

    [SerializeField] public GameObject[] stairs;
    public float speed = 2.0f;
    public float stepHeight = 1.5f;
    public float stepWidth = 11.0f;
    public float threshold = -5.0f;

    private void Start()
    {
        for (int i = 0; i < stairs.Length; i++)
        {
            stairs[i].transform.SetParent(this.transform);
            stairs[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stairs[i].transform.localScale = new Vector3(stepWidth, stepHeight, 1);

            if (i == 0)
                stairs[i].transform.localPosition = new Vector3(0, 0, 0);
            else
            {
                var prestair_y = stairs[i - 1].transform.position.y;
                var prestair_z = stairs[i - 1].transform.position.z;
                stairs[i].transform.localPosition = new Vector3(stairs[i].transform.localPosition.x, prestair_y - stepHeight, prestair_z + stepWidth);
            }
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.y < threshold)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += stepHeight * stairs.Length;
            transform.position = newPosition;
        }
    }


}
