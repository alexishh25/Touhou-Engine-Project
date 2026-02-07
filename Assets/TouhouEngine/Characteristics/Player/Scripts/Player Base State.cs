using UnityEngine;

public abstract class PlayerBaseState 
{
    protected PlayerStateManager _ctx;
    public PlayerBaseState(PlayerStateManager currentcontext)
    {
        _ctx = currentcontext;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void OnCollisionEnter();

    protected void LogicMoverse()
    {
        if (_ctx.moveAction == null) return;

        Vector2 moveInput = _ctx.moveAction.ReadValue<Vector2>();
        moveInput = new Vector2(
            Mathf.Abs(moveInput.x) > 0.1f ? Mathf.Sign(moveInput.x) : 0,
            Mathf.Abs(moveInput.y) > 0.1f ? Mathf.Sign(moveInput.y) : 0
        );

        float velocidadBase = _ctx.currentData.basemoveSpeed;
        float focusAction = _ctx.focusAction.ReadValue<float>();
        float divisor = _ctx.currentData.focusSpeed;

        float velocidadFinal = focusAction == 1.0f ? velocidadFinal = velocidadBase / divisor : velocidadFinal = velocidadBase;
        _ctx.rgb2D.linearVelocity = moveInput * velocidadFinal;

        PlayerAnimationController.Instance.AlterarProgresivamenteBlend(_ctx.animator, "Blend", moveInput);
    }

}
