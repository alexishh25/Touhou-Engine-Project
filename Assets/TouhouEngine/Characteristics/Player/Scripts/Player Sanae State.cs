using UnityEngine;

public class PlayerSanaeState : PlayerBaseState
{
    public PlayerSanaeState(PlayerStateManager currentContext) : base(currentContext) { }
    public override void EnterState()
    {
        Debug.Log("Entering Sanae State");
        CharacterData data = Resources.Load<CharacterData>("SanaeData");

        _ctx.LoadCharacterData(data);
    }
    public override void UpdateState()
    {
        LogicMoverse();
    }
    public override void OnCollisionEnter()
    {
        // Handle Sanae-specific collision logic here
    }
}
