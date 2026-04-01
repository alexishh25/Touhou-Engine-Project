using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum ScreenType
{
    MainMenu,
    SelectCharacter,
    LoadScreen,
    Settings
}

[Serializable]
public struct ScreenEntry
{
    public ScreenType type;
    public VisualTreeAsset visualAsset;
    public ScreenLogic logicComponent;
}
/// <summary>
/// Scene-specific controller for the Menu scene.
/// Owns the screen definitions and registers them with UIManager on startup.
/// </summary>
public class MenuUIController : MonoBehaviour
{
    public static MenuUIController Instance { get; private set; }

    [Header("Screens available in this scene")]
    [SerializeField] private ScreenType initialScreen;
    [SerializeField] private UIDocument uIDocument;
    [SerializeField] private ScreenEntry[] screens;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager not found. Make sure Bootstrap is loaded.");
            return;
        }

        UIManager.Instance.RegisterScreens(screens, initialScreen, uIDocument);
    }
}
