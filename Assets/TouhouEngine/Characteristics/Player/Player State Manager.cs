using System.Globalization;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{

    public static PlayerStateManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
