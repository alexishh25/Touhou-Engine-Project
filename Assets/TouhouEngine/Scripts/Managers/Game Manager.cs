using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InputActionAsset inputActions;

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
    }
}
