using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int playerDamage = 1;

    [SerializeField] private LayerMask iDamageableLayer;
    private bool canAttack = true;

    [Header("VFX")]
    [SerializeField] private ParticleSystem weaponParticle;


    [Header("Hit Feedback")]
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private float timeScalePause = 0.2f;
    [SerializeField] private float timePauseDuration = 0.3f;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Volume Feedback")]
    [SerializeField] private Volume postProcessVolume;
    private ChromaticAberration chromaticAberration;

    [Header("Reference")]
    [SerializeField] private PlayerAnimation playerAnimation;

    private void Start()
    {
        postProcessVolume.profile.TryGet(out chromaticAberration);
    }

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
            Debug.Log("hit");
            if (hit.TryGetComponent<IDamageable>(out var damageable) && !hit.CompareTag("Player"))
            {
                Debug.Log("hit enemy");
                damageable.TakeDamage(playerDamage);
                Vector2 hitPos = hit.ClosestPoint(transform.position);
                AudioManager.instance.PlaySFX(AudioManager.instance.swordHit);
                impulseSource.GenerateImpulse();
                StartCoroutine(HitFeedbackIenumerator());

                Instantiate(hitParticle, hitPos, Quaternion.identity);
            }
        }
    }
    public void OnSwing()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.swordSwing);
        weaponParticle.Play(true);
    }


    IEnumerator HitFeedbackIenumerator()
    {
        chromaticAberration.intensity.value = 0.5f;
        Time.timeScale = timeScalePause;
        yield return new WaitForSecondsRealtime(timePauseDuration);  
        Time.timeScale = 1f;
        chromaticAberration.intensity.value = 0;
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

    private void OnEnable()
    {
        Time.timeScale = 1;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
