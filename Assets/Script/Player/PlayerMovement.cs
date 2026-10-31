using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    private Vector2 direction = Vector2.zero;

    [Header("Dash")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashForced = 20f;
    private bool isDashing;

    [Header("Reference")]
    [SerializeField] private PlayerAnimation playerAnimation;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public void PlayerDash()
    {
        if(isDashing) return;
        AudioManager.instance.PlaySFX(AudioManager.instance.playerDash);
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        PlayerScript.instance.SetImmunity(true);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        PlayerScript.instance.SetImmunity(false);
    }

    private void Update()
    {
        if (isDashing)
            rb.AddForce(direction * dashForced);
        else
            rb.linearVelocity = direction.normalized * moveSpeed;
        if(rb.linearVelocity.magnitude > 0.01)
        {
            playerAnimation.PlayRunAnimation(rb.linearVelocity);
        }
        else
        {
            playerAnimation.PlayIdleAnimation();
        }
    }

}
