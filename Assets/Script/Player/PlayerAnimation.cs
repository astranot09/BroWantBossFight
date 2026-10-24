using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayIdleAnimation()
    {
        animator.SetBool("isMoving", false);
    }
    public void PlayRunAnimation(Vector2 dir)
    {
        animator.SetBool("isMoving", true);
        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);
    }
    public void PlayAttackAnimation()
    {
        animator.SetTrigger("isAttacking");
    }
}
