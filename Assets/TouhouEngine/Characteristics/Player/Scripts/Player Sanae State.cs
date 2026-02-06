using UnityEngine;

public class PlayerSanaeState : PlayerBaseState
{
    public PlayerSanaeState(PlayerStateManager currentContext) : base(currentContext)
    {

    }
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entering Sanae State");
        // Initialize Sanae-specific attributes here
    }
    public override void UpdateState(PlayerStateManager player)
    {
        // Handle Sanae-specific updates here
    }
    public override void OnCollisionEnter(PlayerStateManager player)
    {
        // Handle Sanae-specific collision logic here
    }
}
