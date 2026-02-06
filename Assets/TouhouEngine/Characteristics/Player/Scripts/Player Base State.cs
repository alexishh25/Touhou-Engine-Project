using UnityEngine;

public abstract class PlayerBaseState 
{
    protected PlayerStateManager _ctx;
    public PlayerBaseState(PlayerStateManager currentcontext)
    {
        _ctx = currentcontext;
    }
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnCollisionEnter(PlayerStateManager player);

}
