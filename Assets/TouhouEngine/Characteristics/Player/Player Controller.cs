using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rgb2D;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction focusAction;
    [HideInInspector] public Animator animator;
    private void Awake()
    {
        rgb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerStateManager.Instance.animator = animator;
        PlayerStateManager.Instance.rgb2D = rgb2D;
        moveAction = PlayerStateManager.Instance.moveAction;
        focusAction = PlayerStateManager.Instance.focusAction;
    }

    private void Update()
    {
        if (PlayerStateManager.Instance.currentStats != null)
        {
            PlayerStateManager.Instance.currentStats.UpdateState(); 
        }
    }

}
