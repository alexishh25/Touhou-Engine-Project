using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;

    public InputAction moveAction;

    public Vector2 moveInput;

    private Animator animator;
    private Rigidbody2D rgdbody;

    [SerializeField] public float moveSpeed = 5f;

    private void OnEnable() => inputActions.FindActionMap("Player").Enable();

    private void OnDisable() => inputActions.FindActionMap("Player").Disable();

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
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveAction.WasPressedThisFrame())
        {
            rgdbody.AddForceAtPosition(new Vector2(moveSpeed, 0), Vector2.up, ForceMode2D.Force);
        }
    }
    private void Update()
    {
        //Moverse();
    }
}
