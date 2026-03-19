using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider2D))]
public class BoxColliderFit : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Camera cam;

    [Header("Values")] //Percentage of the screen to offset the collider from each side (0 = no offset, 0.5 = half the screen, 0.99 = almost the entire screen)
    [SerializeField][Range(0f, 0.99f)] private float bottomOffset = 0f;
    [SerializeField][Range(0f, 0.99f)] private float topOffset = 0f;
    [SerializeField][Range(0f, 0.99f)] private float rightOffset = 0f;
    [SerializeField][Range(0f, 0.99f)] private float leftOffset = 0f;

    // Cache of values to detect changes in the camera or screen size
    private float cachedOrthographicSize;
    private float cachedAspect;
    private int cachedScreenWidth;
    private int cachedScreenHeight;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.offset = Vector2.zero;
        cam = Camera.main;
        CacheValues();
        FitToScreen();
    }

    private void CacheValues()
    {
        if (cam == null) return;
        cachedOrthographicSize = cam.orthographicSize;
        cachedAspect = cam.aspect;
        cachedScreenWidth = Screen.width;
        cachedScreenHeight = Screen.height;
    }

    private bool CameraChanged()
    {
        if (cam == null) return false;
        return cam.orthographicSize != cachedOrthographicSize
            || cam.aspect != cachedAspect
            || Screen.width != cachedScreenWidth
            || Screen.height != cachedScreenHeight;
    }

    private void FitToScreen()
    {
        if (cam == null) cam = Camera.main;
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

        CacheValues();
    }

    private void OnValidate()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
        cam = Camera.main;
        CacheValues();
        FitToScreen();
    }

    private void Update()
    {
        if (cam == null) cam = Camera.main;

        if (!Application.isPlaying || CameraChanged())
            FitToScreen();
    }
}