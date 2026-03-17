using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private UIDocument uIDocument;
    private ScreenEntry[] registeredScreens;
    private ScreenLogic currentLogic;
    private VisualElement root;

    [Header("Efectos de sonido")]

    [SerializeField] public AudioClip sfx_buttonhover;
    [SerializeField] public AudioClip sfx_clickbutton;
    [SerializeField] public AudioClip sfx_cancelbutton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (uIDocument != null)
            root = uIDocument.rootVisualElement;
    }

    // Called by UIController in each scene to register available screens
    public void RegisterScreens(ScreenEntry[] screens, ScreenType initialScreen, UIDocument uiDocument)
    {
        registeredScreens = screens;
        uIDocument = uiDocument;
        if (uIDocument != null)
            root = uIDocument.rootVisualElement;

        ChangeScreen(initialScreen);
    }

    public void ChangeScreen(ScreenType type)
    {
        if (currentLogic != null)
            currentLogic.Dispose();

        ScreenEntry entry = Array.Find(registeredScreens, s => s.type == type);
        if (entry.visualAsset == null)
        {
            Debug.LogError($"Screen not found for type: {type}");
            return;
        }

        root.Clear();

        VisualElement screenInstance = entry.visualAsset.Instantiate();
        screenInstance.style.flexGrow = 1f;
        root.Add(screenInstance);

        currentLogic = entry.logicComponent;

        if (currentLogic != null)
            currentLogic.Initialize(screenInstance);
        else
            Debug.LogError($"No script assigned for {type}");
    }

    public void DisableUI()
    {
        if (uIDocument == null) return;
        uIDocument.gameObject.SetActive(false);
    }

    public void EnableUI()
    {
        if (uIDocument == null) return;
        uIDocument.gameObject.SetActive(true);
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
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        asyncLoad.allowSceneActivation = true;
        yield return asyncLoad;

        yield return SceneManager.UnloadSceneAsync("LoadingScreen");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        yield return SceneManager.UnloadSceneAsync(sceneToUnload);

        if (targetScene == "Gameplay")
        {
            yield return new WaitForEndOfFrame();
            InputManager.Instance.SwitchActionMap("Player");
            DisableUI();
            Debug.Log("Action Map 'Player' reactivated after loading Gameplay scene");
        }
    }
}
