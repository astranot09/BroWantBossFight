using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        playerMovement.SetDirection(dir);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Attack");
            playerAttack.OnAttack();
        }
    }

    public void OnDashing(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            playerMovement.PlayerDash();
        }
    }
}
