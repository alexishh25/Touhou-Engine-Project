using UnityEngine;
using UnityEditor;

public class EditorController : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Ejecutar"))
        {
            ((PlayerController)target).CambiarSprites();
        }
    }
}
