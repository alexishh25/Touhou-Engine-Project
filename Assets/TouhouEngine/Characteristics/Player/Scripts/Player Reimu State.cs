using UnityEngine;

public class PlayerReimuState : PlayerBaseState
{
    public PlayerReimuState(PlayerStateManager currentContext) : base(currentContext) { }

    public override void EnterState()
    {
        Debug.Log(message: "Reimu State Ready");

        _ctx.LoadCharacterData(_ctx.currentData);
    }
    public override void UpdateState()
    {
        LogicMoverse();
    }
}
