using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int playerDamage = 1;

    [SerializeField] private LayerMask iDamageableLayer;
    private bool canAttack = true;

    [Header("VFX")]
    [SerializeField] private ParticleSystem weaponParticle;

    [Header("Reference")]
    [SerializeField] private PlayerAnimation playerAnimation;
    public void OnAttack()
    {
        if (!canAttack) return;

        playerAnimation.PlayAttackAnimation();


    }

    public void GiveDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, iDamageableLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable) && !CompareTag("Player"))
            {
                damageable.TakeDamage(playerDamage);
            }
        }
    }
    public void PlayParticle()
    {
        weaponParticle.Play(true);
    }

    public void CanAttack()
    {
        canAttack = true;
    }
    public void DisableAttack()
    {
        canAttack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
    }

}
