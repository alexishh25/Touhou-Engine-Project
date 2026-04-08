using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class LevelTimelineWindow : EditorWindow
{
    private GameplayLevelTimeData levelData;
    private Slider timeSlider;
    private Label timeLabel;

    [MenuItem("Touhou Engine/Timeline Visualizer")]
    public static void ShowWindow()
    {
        LevelTimelineWindow wnd = GetWindow<LevelTimelineWindow>();
        wnd.titleContent = new GUIContent("Timeline Visualizer");
        wnd.minSize = new Vector2(400, 200);
    }

    public void CreateGUI()
    {
        // 1. Cargamos el archivo que diseñaste en UI Builder
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/TouhouEngine/Characteristics/Timeline/Editor/LevelTimelineWindow.uxml");
        
        if (visualTree != null)
        {
            // Pegamos todo el diseño en la ventana
            VisualElement rootFromUXML = visualTree.Instantiate();
            rootVisualElement.Add(rootFromUXML);
        }
        else
        {
            Debug.LogError("No se encontró el archivo UXML.");
            return;
        }

        // 2. BUSCAMOS los elementos visuales por los Nombres que les diste en el UI Builder
        ObjectField levelSelector = rootVisualElement.Q<ObjectField>("levelSelector");
        timeSlider = rootVisualElement.Q<Slider>("timeSlider");
        timeLabel = rootVisualElement.Q<Label>("timeLabel");

        // 3. Agregamos la lógica pura usando código
        if (levelSelector != null)
        {
            levelSelector.objectType = typeof(GameplayLevelTimeData);
            levelSelector.RegisterValueChangedCallback(evt => {
                levelData = (GameplayLevelTimeData)evt.newValue;
                SceneView.RepaintAll();
            });
        }

        if (timeSlider != null)
        {
            timeSlider.RegisterValueChangedCallback(evt => {
                if (timeLabel != null) timeLabel.text = $"Tiempo actual: {evt.newValue:F2}s";
                SceneView.RepaintAll(); 
            });
        }
    }

    // Nos suscribimos a los eventos de la ventana de Escena
    private void OnEnable() { SceneView.duringSceneGui += OnSceneGUI; }
    private void OnDisable() { SceneView.duringSceneGui -= OnSceneGUI; }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (levelData == null || levelData.TimeData == null || timeSlider == null) return;

        float currentTime = timeSlider.value;

        // Limpiar estilos
        Handles.color = Color.magenta;

        // Revisar a cada enemigo que existe en el nivel
        foreach (var data in levelData.TimeData)
        {
            if (data.spawns == null) continue;

            // Si el enemigo ya nació (el tiempo actual del slider superó su momento de spawn)
            if (currentTime >= data.time)
            {
                foreach (var spawn in data.spawns)
                {
                    float lifeTimeOfEnemy = currentTime - data.time;

                    // Si el enemigo ya excedió su tiempo de vida máximo, entonces no lo dibujamos (ya murió)
                    if (lifeTimeOfEnemy > spawn.duration && spawn.duration > 0f) 
                        continue;

                    // Calcular donde ESTARÍA este enemigo según la ruta
                    Vector3 simulatedPos = CalculateGhostPosition(spawn, lifeTimeOfEnemy);

                    // PINTAR EL PREFAB REAL EN LA ESCENA
                    if (spawn.prefab != null)
                    {
                        SpriteRenderer sr = spawn.prefab.GetComponentInChildren<SpriteRenderer>();
                        if (sr != null && sr.sprite != null)
                        {
                            Handles.BeginGUI();
                            Vector3 screenPos = HandleUtility.WorldToGUIPoint(simulatedPos);
                            
                            Sprite sprite = sr.sprite;
                            Texture2D tex = sprite.texture;
                            
                            // CALCULAR EL RECORTE DEL SPRITE (EVITA DIBUJAR LA SPRITESHEET ENTERA)
                            Rect texRect = sprite.textureRect;
                            Rect uvRect = new Rect(
                                texRect.x / tex.width,
                                texRect.y / tex.height,
                                texRect.width / tex.width,
                                texRect.height / tex.height
                            );

                            // Ajustar escala base (ej. altura maxima de 64p) 
                            // y mantener el aspect ratio original del recorte
                            float targetHeight = 64f;
                            float targetWidth = targetHeight * (texRect.width / texRect.height);

                            Rect iconRect = new Rect(screenPos.x - (targetWidth/2), screenPos.y - (targetHeight/2), targetWidth, targetHeight);
                            
                            // Dibujar usando solo la porcion recortada
                            GUI.color = new Color(1, 1, 1, 0.75f);
                            GUI.DrawTextureWithTexCoords(iconRect, tex, uvRect, true);
                            GUI.color = Color.white;
                            Handles.EndGUI();
                        }
                        else
                        {
                            Handles.DrawSolidDisc(simulatedPos, Vector3.forward, 0.4f);
                        }
                        
                        Handles.Label(simulatedPos + Vector3.down * 0.5f, spawn.prefab.name);
                    }
                    else
                    {
                        Handles.DrawSolidDisc(simulatedPos, Vector3.forward, 0.4f);
                        Handles.Label(simulatedPos + Vector3.down * 0.5f, "? SIN PREFAB");
                    }
                }
            }
        }
    }

    private Vector3 CalculateGhostPosition(SpawnInstruction spawn, float lifeTime)
    {
        // Si usa Math / Ecuaciones que agregaste
        if (spawn.movementOrigin == MovementOrigin.MathTrajectory)
        {
            // Ejemplo básico: Mover en linea recta según tiempo y velocidad
            return spawn.position + (spawn.trajectory.targetDirection * spawn.trajectory.speed * lifeTime);
        }
        // Si usa Waypoints (Nuestra opción UI Toolkit)
        else if (spawn.movementOrigin == MovementOrigin.WaypointPath && spawn.pathData != null)
        {
            Vector3[] paths = spawn.pathData.waypoints;
            if (paths.Length == 0) return spawn.position;

            Vector3 currentPos = spawn.position; // Donde inicia
            float timeToSpend = lifeTime;

            // Avanzar matemáticamente nodo por nodo según su vida
            for (int i = 0; i < paths.Length; i++)
            {
                float distToNext = Vector3.Distance(currentPos, paths[i]);
                float timeToReachNext = distToNext / spawn.pathData.moveSpeed;

                if (timeToSpend > timeToReachNext)
                {
                    // Lo pasamos, restamos tiempo y brincamos de nodo
                    timeToSpend -= timeToReachNext;
                    currentPos = paths[i];
                }
                else
                {
                    // Está atrapado en alguna parte de esta línea, aquí está actualmente:
                    Vector3 dir = (paths[i] - currentPos).normalized;
                    return currentPos + (dir * spawn.pathData.moveSpeed * timeToSpend);
                }
            }
            return paths[paths.Length - 1]; // Se quedó en el punto final
        }

        return spawn.position; // Fallback
    }
}
