using UnityEngine;

public class BossScript : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Camera")]
    [SerializeField] private float cameraZoomOut = 5.5f;

    private void Start()
    {
        currentHealth = maxHealth;
        //BossSpecialAttack();
    }

    public void TakeDamage(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            BossDeath();
        }
    }

    public void BossSpecialAttack()
    {
        CinemachineManager.instance.ChangeCamValue(cameraZoomOut);
    }

    public void BossDeath()
    {

    }
}
