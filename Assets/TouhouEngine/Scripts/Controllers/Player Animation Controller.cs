using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    private static readonly int idleHash = Animator.StringToHash("Idle");
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");

    private enum PlayerState
    {
        Idle,
        MovingRight,
        MovingLeft
    }

    [SerializeField] private PlayerState playerState;

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
    private void Update()
    {
        switch (playerState)
        {
            case PlayerState.Idle:

                break;
            case PlayerState.MovingRight:
                // Handle moving right state
                break;
            case PlayerState.MovingLeft:
                // Handle moving left state
                break;
        }
    }
}
