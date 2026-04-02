using UnityEngine;
using Cysharp.Threading.Tasks;

public class RotatingObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float time = 0f;

    [Header("Components")]
    [SerializeField] private float RPS = 0.05f;
    [SerializeField] private bool reverse = false;
    [SerializeField] private bool IsSelected = false;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MoveObj();
    }

    private void MoveObj()
    {
        float gradespersec = RPS * 360f * Time.deltaTime;
        if (IsSelected)
        {
            gradespersec *= 7f;
            time += Time.deltaTime;
            if (time >= 0.3f)
            {
                time = 0f;
                IsSelected = false;
            }
        }


        float direction = reverse ? -gradespersec : gradespersec;
        transform.Rotate(0f, 0f, direction);
    }

}
