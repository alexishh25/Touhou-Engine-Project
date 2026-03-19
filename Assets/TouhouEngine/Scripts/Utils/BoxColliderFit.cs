using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider2D))]
public class BoxColliderFit : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Camera cam;

    [Header("Values")]
    [SerializeField][Range(0f, 0.99f)] private float bottomOffset = 0f;
    [SerializeField][Range(0f, 0.99f)] private float topOffset = 0f;
    [SerializeField][Range(0f, 0.99f)] private float rightOffset = 0f;
    [SerializeField][Range(0f, 0.99f)] private float leftOffset = 0f;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.offset = Vector2.zero;
        cam = Camera.main;
        FitToScreen();
    }

    private void FitToScreen()
    {
        Camera cam = Camera.main;

        if (cam == null) return;

        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        float right = screenWidth * rightOffset;
        float left = screenWidth * leftOffset;
        float top = screenHeight * topOffset;
        float bottom = screenHeight * bottomOffset;

        float finalWidth = screenWidth - right - left;
        float finalHeight = screenHeight - top - bottom;

        float offsetX = (left - right) / 2f;
        float offsetY = (bottom - top) / 2f;

        boxCollider.size = new Vector2(finalWidth, finalHeight);
        boxCollider.offset = new Vector2(offsetX, offsetY);

    }

    private void OnValidate()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
        FitToScreen();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            FitToScreen();
    }
}
