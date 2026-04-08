using UnityEngine;
using UnityEngine.Rendering.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelController levelController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartGameplay();
    }

    public void StartGameplay()
    {
        if (levelController != null)
            levelController.StartLevelPlay();
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Player has died. Restarting level...");
    }
}
