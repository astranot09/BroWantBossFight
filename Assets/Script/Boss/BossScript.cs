using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public enum BossState
{
    Idle,
    Escape,
    SpecialAttack,
    Death    
}

public class BossScript : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Change State")]
    [SerializeField] private float changeStateFromIdleTime = 2f;

    [Header("Camera")]
    [SerializeField] private float cameraZoomOut = 5.5f;

    [Header("Feedback Hurt")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float durationBlink = 0.4f;
    private Coroutine blinkCoroutine;

    [SerializeField] private GameObject popUpDamagePrefab;
    [SerializeField] private float popUpTargetX = 1f;

    [SerializeField] private float punchDuration = 0.2f;
    [SerializeField] private Vector3 punchStrength = new Vector3(-0.1f, -0.1f, 0f);
    [SerializeField] private int punchVibrato = 5;

    private Vector3 originalScale;
    private BossState bossState;
    public BossState CurrentState => bossState;

    [Header("Reference")]
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private BossAttack bossAttack;
    [SerializeField] private BossTeleportAroundScript bossTeleport;

    public event Action OnBossHealthChange;

    private void OnEnable()
    {
        gameplayManager.OnGameStart += BossGameStart;
    }
    private void OnDisable()
    {
        gameplayManager.OnGameStart -= BossGameStart;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        originalScale = transform.localScale;
    }

    private void BossGameStart()
    {
        StartCoroutine(BossStateChangeIenumerator());
    }

    IEnumerator BossStateChangeIenumerator()
    {
        SwitchStateBoss(BossState.Idle);

        yield return new WaitForSeconds(changeStateFromIdleTime);

        RandomizerState();

    }

    private void RandomizerState()
    {
        int x = UnityEngine.Random.Range(0,2); //0 = special attack, 1 = escape
        switch (x)
        {
            case 0:
                Debug.Log("Special Attack Boss");
                SwitchStateBoss(BossState.SpecialAttack);
                break;
            case 1:
                Debug.Log("Boss Teleport Around");
                SwitchStateBoss(BossState.Escape);
                break;
        }
    }

    public void BossSpecialAttack()
    {
        bossAttack.RandomSpecialAttack();
        CinemachineManager.instance.ChangeCamValue(cameraZoomOut);
    }

    public void BossTeleportAround()
    {
        CinemachineManager.instance.ChangeCamValue(cameraZoomOut);
        bossTeleport.TeleportInit();
    }


    // ================== BOSS STATE ====================
    public void SwitchStateBoss(BossState state)
    {
        if (bossState == state) return;

        bossState = state;
        CheckState();
    }

    private void CheckState()
    {
        switch (bossState)
        {
            case BossState.Idle:
                CinemachineManager.instance.ResetCamValue();
                StartCoroutine(BossStateChangeIenumerator());
                break;

            case BossState.SpecialAttack:
                BossSpecialAttack();
                break;

            case BossState.Escape:
                BossTeleportAround();
                break;

            case BossState.Death:
                BossDeath();
                break;
        }
    }


    // ================== TAKING DAMAGE ====================
    public void TakeDamage(int value)
    {
        currentHealth -= value;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        TriggerDamagePopUp(value);
        TriggerBlink();
        PunchAnimation();

        OnBossHealthChange?.Invoke();

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            SwitchStateBoss(BossState.Death);
        }
    }

    public void BossDeath()
    {
        GameStateManager.instance.SwitchState(GameState.PlayerWin);
        gameObject.SetActive(false);
    }


    // ================== FEEDBACK ====================
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
    public void PunchAnimation()
    {
        transform.DOKill();
        transform.localScale = originalScale;

        transform.DOPunchScale(punchStrength, punchDuration, punchVibrato, 1f);
    }

}
