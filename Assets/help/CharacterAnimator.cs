using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 lastMoveDir;
    private bool isRolling;
    private bool isDashing;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayMoveAnimation(Vector2 moveDir)
    {
        if (moveDir != Vector2.zero)
        {
            lastMoveDir = moveDir;
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Flip sprite if moving left
        if (moveDir.x != 0)
        {
            spriteRenderer.flipX = moveDir.x < 0;
        }
    }

    public void PlayRollAnimation(Vector2 rollDir)
    {
        isRolling = true;
        animator.SetTrigger("Roll");

        // Ensure the sprite faces the correct direction during roll
        if (rollDir.x != 0)
        {
            spriteRenderer.flipX = rollDir.x < 0;
        }
    }

    public void PlayDashAnimation()
    {
        if (!isDashing)
        {
            isDashing = true;
            animator.SetTrigger("Dash");
        }
    }

    public void StopRoll()
    {
        isRolling = false;
    }

    public void StopDash()
    {
        isDashing = false;
    }
}
