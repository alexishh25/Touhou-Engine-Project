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

    private void OnEnable() => moveAction?.Enable();

    private void OnDisable() => moveAction?.Disable();

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        focusAction = InputSystem.actions.FindAction("Focus");
        rgdbody = GetComponent<Rigidbody2D>();
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
