using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
using System.Threading;

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

        ChangeScreen(initialScreen, null , true);
    }

    public void ChangeScreen(ScreenType type, TransitionScreenData data = null, bool firstscreen = false)
    {

        // Cancel inputs on the old screen to prevent clicking bugs
        if (currentLogic != null)
        {
            currentLogic.Dispose();
        }

        // Store existing VisualElements so we can remove them later without affecting the new screen
        var oldElements = new List<VisualElement>(root.Children());

        void PerformChange()
        {
            ScreenEntry entry = Array.Find(registeredScreens, s => s.type == type);
            if (entry.visualAsset == null)
            {
                Debug.LogError($"Screen not found for type: {type}");
                return;
            }

            VisualElement screenInstance = entry.visualAsset.Instantiate();
            screenInstance.style.flexGrow = 1f;

            screenInstance.style.position = Position.Absolute;
            screenInstance.style.left = 0;
            screenInstance.style.right = 0;
            screenInstance.style.top = 0;
            screenInstance.style.bottom = 0;

            root.Insert(0, screenInstance);

            currentLogic = entry.logicComponent;

            if (currentLogic != null)
                currentLogic.Initialize(screenInstance);
            else
                Debug.LogError($"No script assigned for {type}");
        }

        void CleanupOldScreens()
        {
            // Remove only the screens that were there before the transition started
            foreach (var el in oldElements)
            {
                if (root.Contains(el))
                    root.Remove(el);
            }
        }

        if (data != null && TransitionManager.Instance != null)
        {
            TransitionManager.Instance.PlayTransition(data, PerformChange, CleanupOldScreens);
        }
        else
        {
            PerformChange();
            CleanupOldScreens();
        }

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
        LoadWithScreen(targetScene).Forget();
    }

    private async UniTaskVoid LoadWithScreen(string targetScene)
    {
        Scene sceneToUnload = SceneManager.GetActiveScene();

        await SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        await UniTask.WaitForSeconds(2.0f);

        asyncLoad.allowSceneActivation = true;
        await asyncLoad;

        await SceneManager.UnloadSceneAsync("LoadingScreen");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        await SceneManager.UnloadSceneAsync(sceneToUnload);

        if (targetScene == "Gameplay")
        {
            await UniTask.WaitForEndOfFrame(this);
            InputManager.Instance.SwitchActionMap("Player");
            DisableUI();
            Debug.Log("Action Map 'Player' reactivated after loading Gameplay scene");
        }
    }
}
