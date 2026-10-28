using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    public static CharacterController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    [SerializeField] private PlayerInput playerInput;

    [Header("Reference")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;

    private void Start()
    {
        DisabledActionMap();
    }

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

    public void ActivatedActionMap()
    {
        playerInput.ActivateInput();
    }
    public void DisabledActionMap()
    {
        playerInput.DeactivateInput();
    }
}
