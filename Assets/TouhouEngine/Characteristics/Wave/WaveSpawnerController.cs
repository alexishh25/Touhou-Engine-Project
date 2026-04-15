using System;
using UnityEngine;

public class WaveSpawnerController : MonoBehaviour
{
    public enum TrajectoryType
    {
        Linear,
        SineWave,
        Sinusoidal
    }

    [Serializable]
    public struct TrajectoryInstruction
    {
        public TrajectoryType type;
        public Vector3 targetDirection;
        public float speed;
        public float amplitude;
        public float frequency;
    }

    [Serializable]
    public struct BulletSpawnInstruction
    {
        public Vector3 velocity;
        public float speed;
        public bool stream;
    }
    public enum PathMovementStyle
    {
        StrictWaypoints,   // Viaja en línea recta (Punto A -> Punto B -> Punto C)
        SmoothBezierCurve  // Genera una curva suave fluyendo por los puntos
    }

    // Este es el ScriptableObject independiente (El "Asset" de la ruta)
    [CreateAssetMenu(fileName = "NewEnemyPath", menuName = "Scriptable Objects/Enemy Path Data", order = 1)]
    public class EnemyPathData : ScriptableObject
    {
        public PathMovementStyle movementStyle;

        [Tooltip("Posiciones relativas o globales por las que pasará")]
        public Vector3[] waypoints;
        public float moveSpeed = 3f;
    }
    [Serializable]
    public enum MovementOrigin
    {
        MathTrajectory,   // El enemigo usará las matemáticas (Opción 1)
        WaypointPath      // El enemigo seguirá una curva o waypoints guardados (Opción 2)
    }
    [Serializable]
    public class TimeDataStruct
    {
        public float time;
        public SpawnInstruction[] spawner;
    }

    [Serializable]
    public class SpawnInstruction
    {
        public GameObject prefab;
        public Vector3 position;
        public Quaternion rotation;

        [Header("Movement Approach")]
        public MovementOrigin movementOrigin; // Interruptor para decidir de dónde saca su movimiento

        [Tooltip("Configuración matemática. Usado si movementOrigin = MathTrajectory")]
        public TrajectoryInstruction trajectory;

        [Tooltip("Asset de ruta dibujada. Usado si movementOrigin = WaypointPath")]
        public EnemyPathData pathData;

        [Header("Life & Combat")]
        public float duration;
        public BulletSpawnInstruction bullet;
    }

    [SerializeField] private AudioController audioController;
    [SerializeField] private TimeDataStruct[] timedata;
}
