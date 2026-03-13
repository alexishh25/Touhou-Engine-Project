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
            Debug.LogWarning($"No se encontró el Action Map: {currentActionMap}. Asegúrate de que el nombre sea correcto y que esté incluido en el InputActionAsset.");
    }

    private void Start()
    {
        foreach (var manager in ManagersToDisable)
        {
            if (manager != null)
                manager.SetActive(false);
            else
                Debug.LogWarning("Uno de los objetos en ManagerstoDisable es null. Asegúrate de asignar todos los objetos correctamente en el inspector.");
        }
    }

    public void SwitchActionMap(string mapName)
    {
        foreach (var map in inputActions.actionMaps)
        {
            map.Disable();
            Debug.Log($"Deshabilitado Action Map: {map.name}"); 
        }

        inputActions.FindActionMap(mapName)?.Enable(); // activa solo el que necesitas

        if (inputActions.FindActionMap(mapName) == null)
            Debug.LogWarning($"No se encontró el Action Map: {mapName}. Asegúrate de que el nombre sea correcto y que esté incluido en el InputActionAsset.");

        currentActionMap = mapName;
    }
}
