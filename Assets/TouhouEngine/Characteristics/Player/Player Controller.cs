using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    public InputAction moveAction, focusAction;

    public Vector2 moveInput;   

    private Rigidbody2D rgdbody;

    [SerializeField] public float moveSpeed;

    private void OnEnable() 
    {
        if (moveAction != null)
        {
            moveAction.Enable();
            Debug.Log("PlayerController: moveAction habilitada");
        }
        if (focusAction != null)
        {
            focusAction.Enable();
            Debug.Log("PlayerController: focusAction habilitada");
        }
    }

    private void OnDisable() 
    {
        if (moveAction != null)
            moveAction.Disable();
        if (focusAction != null)
            focusAction.Disable();
    }

    private void Awake()
    {
        rgdbody = GetComponent<Rigidbody2D>();
        
        // Usar el InputActionAsset del GameManager en lugar de InputSystem.actions
        if (GameManager.Instance != null && GameManager.Instance.inputActions != null)
        {
            var map = GameManager.Instance.inputActions.FindActionMap("Player");
            if (map != null)
            {
                moveAction = map.FindAction("Move");
                focusAction = map.FindAction("Focus");
                
                if (moveAction == null || focusAction == null)
                    Debug.LogWarning("Move or Focus actions not found in InputActionAsset.");
            }
            else
            {
                Debug.LogError("Action Map 'Player' not found in InputActionAsset.");
            }
        }
        else
        {
            Debug.LogError("GameManager.Instance or inputActions is null. Make sure GameManager is in the scene.");
        }
    }

    [ContextMenu("Cambiar Sprites")]
    public void CambiarSprites()
    {
        Debug.Log("Cambiar Sprites");
    }

    private void Moverse()
    {
        if (moveAction == null) return;

        moveInput = moveAction.ReadValue<Vector2>();

        //Normlizar el input para que solo tome valores -1, 0, 1
        moveInput = new Vector2(
            Mathf.Abs(moveInput.x) > 0.1f ? Mathf.Sign(moveInput.x) : 0,
            Mathf.Abs(moveInput.y) > 0.1f ? Mathf.Sign(moveInput.y) : 0
        );

        float velocidadFinal = focusAction.ReadValue<float>() == 1.0f ? velocidadFinal = moveSpeed / 2.3f : velocidadFinal = moveSpeed;
        rgdbody.linearVelocity = moveInput * velocidadFinal;

        PlayerAnimationController.Instance.AlterarProgresivamenteBlend(GetComponent<Animator>(), "Blend", moveInput);
    }
    private void Update()
    {
        Moverse();
    }
}
