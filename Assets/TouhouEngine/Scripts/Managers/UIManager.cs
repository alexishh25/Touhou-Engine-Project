using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum ScreenType
{
    MainMenu,
    SelectCharacter,
    LoadScreen
}

[Serializable]
public struct ScreenEntry
{
    public ScreenType type;
    public VisualTreeAsset visualAsset;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Documento Principal")]
    [SerializeField] private UIDocument uIDocument;

    [Header("Lista de screens (uxml)")]
    [SerializeField] public ScreenEntry[] screens;

    private VisualElement root;
    public InputActionAsset navigateActions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        root = uIDocument.rootVisualElement;
    }
    private void Start()
    {
        ChangeScreen(ScreenType.MainMenu);
        navigateActions.FindActionMap("UI").Enable();
    }

    public void ChangeScreen(ScreenType type)
    {
        ScreenEntry entry = Array.Find(screens, s => s.type == type);
        if (entry.visualAsset == null)
        {
            Debug.LogError($"No se encontró el screen para el tipo: {type}");
            return;
        }

        root.Clear();

        VisualElement screenInstance = entry.visualAsset.Instantiate();
        screenInstance.style.flexGrow = 1f;
        root.Add(screenInstance);

        InitializeCurrentScreen(type, screenInstance);
    }



    private void InitializeCurrentScreen(ScreenType type, VisualElement screenRoot)
    {
        switch (type)
        {
            case ScreenType.MainMenu:
                var menuLogic = FindFirstObjectByType<MenuScript>();
                if (menuLogic != null)
                    menuLogic.Setup(screenRoot);
                else 
                    Debug.LogError("No se encontró el MenuScript para configurar los botones del MainMenu.");
                break;
            case ScreenType.SelectCharacter:
                SetupOptions(screenRoot);
                break;
            default:
                Debug.LogWarning($"No se ha implementado la inicialización para el screen: {type}");
                break;
        }
    }

    private void SetupOptions(VisualElement screenRoot)
    {
        // Lógica para la pantalla de opciones
        var backBtn = screenRoot.Q<Button>("BackButton");
        if (backBtn != null)
        {
            backBtn.clicked += () => ChangeScreen(ScreenType.MainMenu);
            backBtn.Focus();
        }
    }
}
