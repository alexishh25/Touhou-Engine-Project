using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootStrap
{
    const string SceneName = "Bootstrap";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
        {
            var candidateScene = SceneManager.GetSceneAt(sceneIndex);

            if (candidateScene.name == SceneName) return;
        }

        // Load Bootstrap additively so managers initialize
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}

/// <summary>
/// Ensures managers are always loaded regardless of which scene is opened first in the editor.
/// Attach this to any scene that requires persistent managers.
/// </summary>
public class BootstrapLoader : MonoBehaviour
{
    public static BootstrapLoader Instance { get; private set; }
    private void Awake()
    {
        // If GameManager already exists, Bootstrap was already loaded
        if (Instance != null)
        {
            Debug.LogWarning($"Multiple instances of BootstrapLoader detected! This should not happen. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
