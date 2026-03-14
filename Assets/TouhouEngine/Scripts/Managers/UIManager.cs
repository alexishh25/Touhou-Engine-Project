using System;
using System.Collections;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    }

    public void ChangeScreen(ScreenType type)
    {
        if (currentEntry.logicComponent != null)
            currentEntry.logicComponent.Dispose();

        ScreenEntry entry = Array.Find(screens, s => s.type == type);
        if (entry.visualAsset == null)
        {
            Debug.LogError($"Screen not found for type: {type}");
            return;
        }

        root.Clear();

        VisualElement screenInstance = entry.visualAsset.Instantiate();
        screenInstance.style.flexGrow = 1f;
        root.Add(screenInstance);

        InitializeCurrentScreen(type, screenInstance);
    }

    public void InterpolateScreenLoad(string targetScene)
    {
        StartCoroutine(LoadWithScreen(targetScene));
    }

    private IEnumerator LoadWithScreen(string targetScene)
    {
        Scene sceneToUnload = SceneManager.GetActiveScene();

        yield return SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) 
        {
            float progress = asyncLoad.progress / 0.9f;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        asyncLoad.allowSceneActivation = true;
        yield return null;

        yield return SceneManager.UnloadSceneAsync("LoadingScreen");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        yield return SceneManager.UnloadSceneAsync(sceneToUnload);
        
        // Ensure the "Player" Action Map is active after loading the Gameplay scene
        if (targetScene == "Gameplay")
        {
            yield return new WaitForEndOfFrame(); // Wait one frame for objects to initialize
            GameManager.Instance.SwitchActionMap("Player");
            UIManager.Instance.DisableUI();

            Debug.Log("Action Map 'Player' reactivated after loading Gameplay scene");
        }
    }

    private void InitializeCurrentScreen(ScreenType type, VisualElement screenRoot)
    {
        ScreenEntry entry = Array.Find(screens, s => s.type == type);

        if (entry.logicComponent != null)
            entry.logicComponent.Initialize(screenRoot);
        else
            Debug.LogError($"No script assigned for {type}");
    }

}
