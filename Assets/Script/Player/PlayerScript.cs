using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;


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
    public int MaxHealth => maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Immunity")]
    [SerializeField] private bool isImmune;
    [SerializeField] private float timePauseDuration = 0.3f;
    [SerializeField] private float timeScalePause = 0.1f;
    [SerializeField] private ParticleSystem parryVFX;

    [Header("Light Feedback")]
    [SerializeField] private Light2D light2D;

    [Header("Feedback Hurt")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float durationBlink = 0.4f;
    private Coroutine blinkCoroutine;

    [SerializeField] private GameObject popUpDamagePrefab;
    [SerializeField] private float popUpTargetX = 1f;

    [SerializeField] private CinemachineImpulseSource impulseSource;


    [Header("VFX")]
    [SerializeField] private GameObject deathVFX;

    public event Action OnPlayerHealthChange;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        GameplayManager.instance.OnGameStart += PlayerInit;
    }
    private void OnDisable()
    {
        GameplayManager.instance.OnGameStart -= PlayerInit;
    }


    private void PlayerInit()
    {
        currentHealth = maxHealth;
        CinemachineManager.instance.AddTarget(this.transform);
    }

    public void PlayerDeath()
    {
        Instantiate(deathVFX,transform.position,Quaternion.identity);
        GameStateManager.instance.SwitchState(GameState.PlayerLose);
        gameObject.SetActive(false);
    }

    public void TakeDamage(int value)
    {
        if (isImmune)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.playerParry);
            StartCoroutine(ParryFeedbackIenumerator());
            parryVFX.Play(true);
            return;
        }

        currentHealth -= value;
        AudioManager.instance.PlaySFX(AudioManager.instance.playerHurt);

        TriggerDamagePopUp(value);
        TriggerBlink();
        TriggerCameraShake();

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnPlayerHealthChange?.Invoke();

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
    private void TriggerCameraShake()
    {
        impulseSource.GenerateImpulse();
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
    IEnumerator ParryFeedbackIenumerator()
    {
        Time.timeScale = timeScalePause;
        yield return new WaitForSecondsRealtime(timePauseDuration);
        Time.timeScale = 1f;
    }
}
