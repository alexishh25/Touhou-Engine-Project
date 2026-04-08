using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private GameplayLevelTimeData leveldata;

    private float currentTime = 0f;
    private int currentEventIndex = 0;
    private bool isPlaying = false;

    public void StartLevelPlay()
    {
        currentTime = 0f;
        currentEventIndex = 0;
        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying || leveldata == null || currentEventIndex >= leveldata.TimeData.Length)
            return;

        currentTime += Time.deltaTime;

        while (currentEventIndex < leveldata.TimeData.Length &&
            currentTime >= leveldata.TimeData[currentEventIndex].time)
        {
            ExecuteSpawns(leveldata.TimeData[currentEventIndex].spawns);
            currentEventIndex++;
        }

    }

    private void ExecuteSpawns(SpawnInstruction[] spawns)
    {
        foreach (var instruction in spawns)
        {
            if (instruction.prefab != null)
            {
                GameObject enemyInstance =
                    Instantiate(instruction.prefab, instruction.position,
                    instruction.rotation);

                if (enemyInstance.TryGetComponent<EnemyShooter>(out var enemyShooter))
                    enemyShooter.InitializeFromTimeline(instruction);
            }
                
        }
    }
}
