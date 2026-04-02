using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CamPosition : MonoBehaviour
{
    [Header("Camera values")]
    [SerializeField] private Camera cam;

    [Header("Object values")]
    [SerializeField] private Renderer rend;

    #region Position
    [SerializeField][Range(0f, 1f)] private float positionx = 0.5f;
    [SerializeField][Range(0f, 1f)] private float positiony = 0.5f;
    #endregion

    #region Anchor
    [SerializeField][Range(0f, 1f)] private float anchorx = 0.5f;
    [SerializeField][Range(0f, 1f)] private float anchory = 0.5f;
    #endregion

    private void PosObject()
    {
        Vector3 pos = cam.ViewportToWorldPoint(
            new Vector3(positionx, positiony, transform.position.z)
        );
        transform.position = AnchorRender(pos);
    }

    private Vector3 AnchorRender(Vector3 posObject)
    {
        Vector3 newpos = posObject;

        if (rend == null)
            return posObject;
        Vector3 size = rend.bounds.size;

        float offsetX = (anchorx - 0.5f) * size.x;
        float offsetY = (anchory - 0.5f) * size.y;

        newpos -= new Vector3(offsetX, offsetY, newpos.z);
        return newpos;
    }

    private void OnValidate()
    {
        PosObject();
    }
    private void Update()
    {
        PosObject();
    }
}
