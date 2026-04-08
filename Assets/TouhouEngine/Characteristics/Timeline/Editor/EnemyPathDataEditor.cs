using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

// 1. Esto le dice a Unity: "Cuando selecciones un EnemyPathData, usa ESTA interfaz".
[CustomEditor(typeof(EnemyPathData))]
public class EnemyPathDataEditor : Editor
{
    private EnemyPathData pathData;

    private void OnEnable()
    {
        pathData = (EnemyPathData)target;
    }

    // ==========================================
    // PARTE 1: LA INTERFAZ DE UI TOOLKIT
    // ==========================================
    public override VisualElement CreateInspectorGUI()
    {
        // Creamos la raíz de nuestro UI Toolkit
        VisualElement root = new VisualElement();

        // 1. Título bonito
        Label title = new Label("Diseñador de Rutas (UI Toolkit)");
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.fontSize = 14;
        title.style.marginBottom = 10;
        root.Add(title);

        // 2. Traer las variables automáticas de tu ScriptableObject
        var styleProp = new PropertyField(serializedObject.FindProperty("movementStyle"), "Estilo de Curva");
        var speedProp = new PropertyField(serializedObject.FindProperty("moveSpeed"), "Velocidad de Movimiento");
        var waypointsProp = new PropertyField(serializedObject.FindProperty("waypoints"), "Nodos de Ruta");

        // Agregarlos al árbol visual
        root.Add(styleProp);
        root.Add(speedProp);
        root.Add(waypointsProp);

        // 3. Botón personalizado (Ejemplo extra)
        Button addPointButton = new Button();
        addPointButton.text = "Añadir Nuevo Punto al Final";
        addPointButton.style.marginTop = 15;
        addPointButton.style.backgroundColor = new Color(0.2f, 0.4f, 0.2f);
        addPointButton.clicked += () => 
        {
            // Lógica al dar clic: agregamos un vector en (0,0,0)
            Undo.RecordObject(pathData, "Add waypoint");
            ArrayUtility.Add(ref pathData.waypoints, Vector3.zero);
            EditorUtility.SetDirty(pathData);
            serializedObject.Update(); // Refrescar el UI
        };
        root.Add(addPointButton);

        return root;
    }

    // ==========================================
    // PARTE 2: LA GIZMO EN LA PANTALLA DE JUEGO (TESTEO VISUAL)
    // ==========================================
    private void OnSceneGUI()
    {
        if (pathData == null || pathData.waypoints == null || pathData.waypoints.Length == 0)
            return;

        // Color verde de matrix
        Handles.color = Color.green;

        // 1. Dibujamos las casillas para arrastrar los PUNTOS con el Ratón
        for (int i = 0; i < pathData.waypoints.Length; i++)
        {
            // Registrar cambio para usar Ctrl+Z
            EditorGUI.BeginChangeCheck();
            
            // Creamos un objeto de "arrastrar" nativo en tu Escena
            Vector3 newPosition = Handles.PositionHandle(pathData.waypoints[i], Quaternion.identity);
            
            // Le ponemos etiqueta de "Punto 0", "Punto 1", etc.
            Handles.Label(pathData.waypoints[i] + Vector3.up * 0.3f, "Punto " + i);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pathData, "Move Waypoint");
                pathData.waypoints[i] = newPosition;
                EditorUtility.SetDirty(pathData); // Gurda fisicamente en el archivo
            }
        }

        // 2. Dibujamos LA LÍNEA que conecta los puntos
        if (pathData.waypoints.Length >= 2)
        {
            if (pathData.movementStyle == PathMovementStyle.StrictWaypoints)
            {
                // Linea recta
                Handles.DrawPolyLine(pathData.waypoints);
            }
            else if (pathData.movementStyle == PathMovementStyle.SmoothBezierCurve)
            {
                // Curva Bézier simplificada (Uniendo con curvas Bezier elásticas)
                for (int i = 0; i < pathData.waypoints.Length - 1; i++)
                {
                    Vector3 start = pathData.waypoints[i];
                    Vector3 end = pathData.waypoints[i + 1];
                    Vector3 control1 = start + (end - start) * 0.25f + Vector3.up; 
                    Vector3 control2 = end - (end - start) * 0.25f + Vector3.up;
                    
                    Handles.DrawBezier(start, end, control1, control2, Color.cyan, null, 2f);
                }
            }
        }
    }
}
