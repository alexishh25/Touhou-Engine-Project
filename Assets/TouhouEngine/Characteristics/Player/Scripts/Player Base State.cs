using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract template that defines the basic structure of a player state.
/// Each specific state (Reimu, Marisa, Sanae, etc.) inherits from this class and implements its own logic for movement, attacking, etc.
/// </summary>
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

        LogicFocus(focusAction);

        float velocidadFinal = focusAction == 1.0f ? velocidadFinal = velocidadBase / divisor : velocidadFinal = velocidadBase;
        _ctx.rgb2D.linearVelocity = moveInput * velocidadFinal;

        PlayerAnimationController.Instance.AlterarProgresivamenteBlend(_ctx.animator, "Blend", moveInput);
    }

    private void LogicFocus(float focusInput)
    {
        bool shouldFocus = focusInput == 1.0f;

        // Only trigger fade when focus state actually changes
        if (shouldFocus == _ctx.isFocus) return;

        _ctx.isFocus = shouldFocus;

        if (_ctx.fadeCoroutine != null)
            _ctx.StopCoroutine(_ctx.fadeCoroutine);

        _ctx.fadeCoroutine = _ctx.StartCoroutine(FadeBox(_ctx.isFocus));
    }

    protected IEnumerator FadeBox(bool @switch)
    {
        float startAlpha = _ctx.spriteRenderer.color.a;
        float targetAlpha = @switch ? 1f : 0f;
        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            Color c = _ctx.spriteRenderer.color;
            _ctx.spriteRenderer.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        Color final = _ctx.spriteRenderer.color;
        _ctx.spriteRenderer.color = new Color(final.r, final.g, final.b, targetAlpha);
        _ctx.fadeCoroutine = null;
    }

}
