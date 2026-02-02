using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    private static readonly int idleHash = Animator.StringToHash("Idle");
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateMovementAnimation(float horizontalInput)
    {
        bool isMoving = Mathf.Abs(horizontalInput) == 0f;

        _animator.SetBool(idleHash, isMoving);
        _animator.SetFloat(isMovingHash, horizontalInput);
    }

    public void PlayDeath()
    {
        _animator.SetTrigger("Death");
    }
}
