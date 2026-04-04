using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject[] managersToDisable;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var manager in managersToDisable)
        {
            if (manager != null)
                manager.SetActive(false);
            else
                Debug.LogWarning("GameManager: A manager in managersToDisable is null.");
        }

        SettingsManager.LoadSettings();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        InputManager.Instance.SwitchActionMap("UI");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        InputManager.Instance.SwitchActionMap("Gameplay");
    }
}
