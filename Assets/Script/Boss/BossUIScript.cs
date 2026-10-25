using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUIScript : MonoBehaviour
{
    [SerializeField] private BossScript bossScript;

    [Header("UI - Panel Boss Health")]
    [SerializeField] private GameObject bossHealthPanel;

    [Header("UI - Boss Health")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider easeHealthBar;
    [SerializeField] private float lerpSpeed = 0.05f;

    private void OnEnable()
    {
        bossScript.OnBossHealthChange += UpdateHealthUI;
        GameplayManager.instance.OnGameStart += BossUISetUp;
    }

    private void OnDisable()
    {
        bossScript.OnBossHealthChange -= UpdateHealthUI;
        GameplayManager.instance.OnGameStart -= BossUISetUp;
    }

    private void Update()
    {
        if(healthBar.value != easeHealthBar.value)
            easeHealthBar.value = Mathf.Lerp(easeHealthBar.value, bossScript.CurrentHealth, lerpSpeed);
    }

    private void BossUISetUp()
    {
        UIManager.instance.OpenPanel(bossHealthPanel);

        healthBar.maxValue = bossScript.MaxHealth;
        easeHealthBar.maxValue = bossScript.MaxHealth;

        healthBar.value = bossScript.CurrentHealth;
        easeHealthBar.value = bossScript.CurrentHealth;
    }

    private void UpdateHealthUI()
    {
        healthBar.maxValue = bossScript.MaxHealth;
        healthBar.value = bossScript.CurrentHealth;
        healthText.text = $"{bossScript.CurrentHealth}/{bossScript.MaxHealth}";
    }
}
