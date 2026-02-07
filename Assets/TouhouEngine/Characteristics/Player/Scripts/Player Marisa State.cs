using UnityEngine;

public class PlayerMarisaState : PlayerBaseState
{
    public PlayerMarisaState(PlayerStateManager currentContext) : base(currentContext) { }
    public override void EnterState()
    {
        Debug.Log("Entering Sanae State");

        _ctx.LoadCharacterData(_ctx.currentData);
    }
    public override void UpdateState()
    {
        LogicMoverse();
    }
    public override void OnCollisionEnter()
    {
        // Handle Marisa-specific collision logic here
    }
}
