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
    public ScreenLogic logicComponent;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Documento Principal")]
    [SerializeField] private UIDocument uIDocument;

    [Header("Lista de screens (uxml)")]
    [SerializeField] public ScreenEntry[] screens;
    private ScreenEntry currentEntry;

    private VisualElement root;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        root = uIDocument.rootVisualElement;
    }

    public void DisableUI()
    {
        uIDocument.gameObject.SetActive(false);
    }

    public void EnableUI()
    {
        uIDocument.gameObject.SetActive(true);
    }
    private void Start()
    {
        ChangeScreen(ScreenType.MainMenu);
        GameManager.Instance.SwitchActionMap("UI");
    }

    public void ChangeScreen(ScreenType type)
    {
        if (currentEntry.logicComponent != null)
            currentEntry.logicComponent.Dispose();

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
        ScreenEntry entry = Array.Find(screens, s => s.type == type);

        if (entry.logicComponent != null)
            entry.logicComponent.Initialize(screenRoot);
        else
            Debug.LogError($"No hay script asignado para {type}");
    }

}
