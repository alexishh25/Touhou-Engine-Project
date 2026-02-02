using UnityEngine;
using UnityEditor;
using System.Collections;
using NUnit.Framework;

public class PlayerController : MonoBehaviour
{

    private enum PlayerState
    {
        Idle,
        MovingRight,
        MovingLeft
    }

    [SerializeField] private PlayerState playerState;


    private void Update()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                
                break;
            case PlayerState.MovingRight:
                // Handle moving right state
                break;
            case PlayerState.MovingLeft:
                // Handle moving left state
                break;
        }
    }   
    [ContextMenu("Cambiar Sprites")]
    public void CambiarSprites()
    {
        Debug.Log("Cambiar Sprites");
    }
}
