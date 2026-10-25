using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable
{
    public static PlayerScript instance;

    private void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Immunity")]
    [SerializeField] private bool isImmune;

    [Header("VFX")]
    [SerializeField] private GameObject deathVFX;


    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void PlayerDeath()
    {
        Instantiate(deathVFX,transform.position,Quaternion.identity);
    }

    public void TakeDamage(int value)
    {
        if (isImmune) return;

        currentHealth -= value;
        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            PlayerDeath();
        }
    }

    public void SetImmunity(bool x)
    {
        isImmune = x;
    }
}
