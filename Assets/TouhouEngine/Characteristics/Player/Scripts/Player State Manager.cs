using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

/// <summary>
/// The brain of the player states. Contains references to necessary components, character data, and instances of each specific state.
/// Handles updating the current state and switching between states when necessary.
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rgb2D;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction focusAction;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CancellationTokenSource fadeCts;

    #region References
    [Header("Components")]
    public CharacterData currentData;
    public BulletManager bulletManager;
    public SpriteRenderer spriteRenderer;
    public bool isFocus = false;
    #endregion

    public PlayerBaseState currentStats;
    public PlayerReimuState reimuState;
    public PlayerMarisaState marisaState;
    public PlayerSanaeState sanaeState;

    private void Awake()
    {
        rgb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // GameManager is guaranteed to exist by Start() since Bootstrap loads additively before scene Awake/Start
        var map = InputManager.Instance.inputActions.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        focusAction = map.FindAction("Focus");

        if (moveAction == null || focusAction == null)
            Debug.LogWarning("Move or Focus actions not found in InputActionAsset. Make sure they are correctly configured.");

        moveAction?.Enable();
        focusAction?.Enable();

        reimuState = new PlayerReimuState(this);
        marisaState = new PlayerMarisaState(this);
        sanaeState = new PlayerSanaeState(this);

        currentStats = reimuState;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        currentStats.EnterState();
    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        focusAction?.Disable();
    }

    public void LoadCharacterData(CharacterData data)
    {
        if (data == null)
        {
            Debug.LogError("Attempted to load null data in StateManager!");
            return;
        }

        currentData = data;

        if (bulletManager != null && data.bulletSprite != null)
            bulletManager.SetAmmoSprite(data.bulletSprite);
        else
            Debug.LogWarning("Missing BulletManager or bullet Sprite assignment in Data");

        if (currentData.animatorController != null)
            animator.runtimeAnimatorController = data.animatorController;
    }

    private void Update()
    {
        currentStats.UpdateState();
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentStats = state;
        state.EnterState();
    }
}
