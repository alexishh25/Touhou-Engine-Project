using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyPathData assignedPath;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    // Esta función la llamará tu LevelController en el instante en que el enemigo NACE
    public void StartFollowingPath(EnemyPathData pathData)
    {
        if (pathData == null || pathData.waypoints.Length == 0) return;

        assignedPath = pathData;
        currentWaypointIndex = 0;

        // Opcional: Teletransportar al enemigo al primer punto de la ruta inmediatamente
        // transform.position = assignedPath.waypoints[0];
        
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving || assignedPath == null) return;

        // Obtenemos el punto al que tenemos que ir
        Vector3 targetPoint = assignedPath.waypoints[currentWaypointIndex];

        // Nos movemos hacia ese punto a la velocidad dictada por el ScriptableObject
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPoint, 
            assignedPath.moveSpeed * Time.deltaTime
        );

        // Si ya llegamos casi exacto a ese punto cambiamos de objetivo
        if (Vector3.Distance(transform.position, targetPoint) <= 0.05f)
        {
            if (currentWaypointIndex < assignedPath.waypoints.Length - 1)
            {
                currentWaypointIndex++; // Avanzamos al siguiente nodo!
            }
            else
            {
                // Llegamos al FINAL de la ruta. En Touhou usualmente el enemigo huye o muere.
                isMoving = false;
                // Destroy(gameObject); 
            }
        }
    }
}
