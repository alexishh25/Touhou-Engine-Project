using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InputActionAsset inputActions;

    [SerializeField] private string currentActionMap = "";
    [SerializeField] private GameObject[] ManagersToDisable;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (currentActionMap != null)
            SwitchActionMap(currentActionMap);
        else if (currentActionMap == "" || currentActionMap == null)
            Debug.LogWarning($"Action Map not found: {currentActionMap}. Make sure the name is correct and included in the InputActionAsset.");
    }

    private void Start()
    {
        foreach (var manager in ManagersToDisable)
        {
            if (manager != null)
                manager.SetActive(false);
            else
                Debug.LogWarning("One of the objects in ManagersToDisable is null. Make sure all objects are correctly assigned in the inspector.");
        }
    }

    public void SwitchActionMap(string mapName)
    {
        foreach (var map in inputActions.actionMaps)
        {
            map.Disable();
            Debug.Log($"Disabled Action Map: {map.name}"); 
        }

        inputActions.FindActionMap(mapName)?.Enable(); // activa solo el que necesitas

        if (inputActions.FindActionMap(mapName) == null)
            Debug.LogWarning($"Action Map not found: {mapName}. Make sure the name is correct and included in the InputActionAsset.");

        currentActionMap = mapName;
    }
}
