using UnityEngine;

public class PlayerReimuState : PlayerBaseState
{
    public PlayerReimuState(PlayerStateManager currentContext) : base(currentContext) { }

    public override void EnterState()
    {
        Debug.Log("Reimu State Ready");

        _ctx.LoadCharacterData(_ctx.currentData);
    }
    public override void UpdateState()
    {
        LogicMoverse();
    }
    public override void OnCollisionEnter()
    {
        // Handle Reimu-specific collision logic here
    }
}
