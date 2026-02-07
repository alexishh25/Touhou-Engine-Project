using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Es el cerebro de los estados del jugador. Contiene referencias a los componentes necesarios, los datos del personaje y las instancias de cada estado específico. 
/// Se encarga de actualizar el estado actual y de cambiar entre estados cuando sea necesario.
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rgb2D;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction focusAction;
    [HideInInspector] public Animator animator;

    #region 1. References
    [Header("Components")]
    public CharacterData currentData;
    public BulletManager bulletManager;
    #endregion

    public PlayerBaseState currentStats;
    public PlayerReimuState reimuState;
    public PlayerMarisaState marisaState;
    public PlayerSanaeState sanaeState;

    private void Awake()
    {
        rgb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        var map = InputSystem.actions.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        focusAction = map.FindAction("Focus");

        reimuState = new PlayerReimuState(this);
        marisaState = new PlayerMarisaState(this);
        sanaeState = new PlayerSanaeState(this);

    }
    private void OnEnable()
    {
        moveAction.Enable();
        focusAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        focusAction.Disable();
    }

    private void Start()
    {
        currentStats = reimuState;
        currentStats.EnterState(); 
    }

    public void LoadCharacterData(CharacterData data)
    {
        if (data == null)
        {
            Debug.LogError("¡Se intentó cargar datos nulos en el StateManager!");
            return;
        }

        currentData = data;

        if (bulletManager != null && data.bulletSprite != null)
            bulletManager.SetAmmoSprite(data.bulletSprite);
        else
            Debug.LogWarning("Falta asignar el BulletManager o el Sprite de bala en el Data");

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
