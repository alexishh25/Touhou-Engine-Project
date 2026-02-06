using System.Globalization;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    public PlayerReimuState reimuState = new PlayerReimuState();
    public PlayerMarisaState marisaState = new PlayerMarisaState();
    public PlayerSanaeState sanaeState = new PlayerSanaeState();

    private void Start()
    {
        currentState = reimuState;
        currentState.EnterState(this); 
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
