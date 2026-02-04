using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    public InputAction moveAction;

    public Vector2 moveInput;

    private Animator animator;
    private Rigidbody2D rgdbody;

    [SerializeField] public float moveSpeed;

    private void OnEnable() => moveAction?.Enable();

    private void OnDisable() => moveAction?.Disable();

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        animator = GetComponent<Animator>();
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

        moveInput = new Vector2(
            Mathf.Abs(moveInput.x) > 0.1f ? Mathf.Sign(moveInput.x) : 0,
            Mathf.Abs(moveInput.y) > 0.1f ? Mathf.Sign(moveInput.y) : 0
        );

        rgdbody.linearVelocity = moveInput * moveSpeed;

        animator.SetFloat("Blend", moveInput.x);
    }
    private void Update()
    {
        Moverse();
    }
}
