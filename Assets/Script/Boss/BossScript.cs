using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public enum BossState
{
    Idle,
    Attack,
    SpecialAttack
}

public class BossScript : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Camera")]
    [SerializeField] private float cameraZoomOut = 5.5f;

    [Header("Feedback Hurt")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float durationBlink = 0.4f;
    private Coroutine blinkCoroutine;
    [SerializeField] private GameObject popUpDamagePrefab;
    [SerializeField] private float popUpTargetX = 1f;

    public event Action OnBossHealthChange;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int value)
    {
        currentHealth -= value;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        TriggerDamagePopUp(value);
        TriggerBlink();

        OnBossHealthChange?.Invoke();

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            BossDeath();
        }
    }

    private void TriggerDamagePopUp(int value)
    {
        float randomX = UnityEngine.Random.Range(-popUpTargetX, popUpTargetX);
        Vector2 posSpawn = new Vector2(transform.position.x + randomX, transform.position.y + 0.5f);
        GameObject x = Instantiate(popUpDamagePrefab, posSpawn, Quaternion.identity);
        x.GetComponent<PopUpDamage>().SetUpPopUpDamage(value);
    }
    private void TriggerBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(SpriteBlink());
    }

    private IEnumerator SpriteBlink()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(durationBlink);
        spriteRenderer.color = Color.white;
        blinkCoroutine = null;
    }


    public void BossSpecialAttack()
    {
        CinemachineManager.instance.ChangeCamValue(cameraZoomOut);
    }

    public void BossDeath()
    {
        GameStateManager.instance.SwitchState(GameState.PlayerWin);
    }
}
