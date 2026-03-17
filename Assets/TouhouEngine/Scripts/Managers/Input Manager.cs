using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] public InputActionAsset inputActions;
    [SerializeField] private string initialActionMap = "";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(initialActionMap))
            SwitchActionMap(initialActionMap);
        else
            Debug.LogWarning("InputManager: No initial action map assigned.");
    }

    public void SwitchActionMap(string mapName)
    {
        foreach (var map in inputActions.actionMaps)
            map.Disable();

        var target = inputActions.FindActionMap(mapName);

        if (target == null)
        {
            Debug.LogWarning($"InputManager: Action Map '{mapName}' not found.");
            return;
        }

        target.Enable();
        Debug.Log($"InputManager: Switched to Action Map '{mapName}'.");
    }

    // Acceso a acciones individuales para suscribirse desde otros scripts
    public InputAction GetAction(string mapName, string actionName)
    {
        var action = inputActions.FindActionMap(mapName)?.FindAction(actionName);

        if (action == null)
            Debug.LogWarning($"InputManager: Action '{actionName}' not found in map '{mapName}'.");

        return action;
    }
}
