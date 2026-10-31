using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIGameplayManager : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;


    [Header("UI - Screen")]
    [SerializeField] private GameObject uiScreen;

    [Header("UI - Player Health")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider easeHealthBar;
    [SerializeField] private float lerpSpeed;

    [Header("Player Hurt Feedback")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private float vignetteSpeed = 10f;
    [SerializeField] private float vignetteFadeSpeed = 0.2f;
    [SerializeField] private float maxVignetteIntensity = 0.125f; // Intensity saat kena damage
    private Vignette vignette;
    private float targetVignetteIntensity = 0;
    private void OnEnable()
    {
        playerScript.OnPlayerHealthChange += UpdateHealthUI;
        GameplayManager.instance.OnGameStart += PlayerUISetUp;
    }

    private void OnDisable()
    {
        playerScript.OnPlayerHealthChange -= UpdateHealthUI;
        GameplayManager.instance.OnGameStart -= PlayerUISetUp;
    }
    private void Start()
    {
        if (postProcessVolume.profile.TryGet(out vignette))
        {
            vignette.active = true;
            vignette.intensity.overrideState = true;
        }
        healthBar.maxValue = playerScript.MaxHealth;
        healthBar.value = playerScript.CurrentHealth;
        healthText.text = $"{playerScript.CurrentHealth}/{playerScript.MaxHealth}";
    }

    private void Update()
    {
        easeHealthBar.value = Mathf.Lerp(easeHealthBar.value, playerScript.CurrentHealth, lerpSpeed);
        targetVignetteIntensity = Mathf.MoveTowards(targetVignetteIntensity, 0f, Time.deltaTime * vignetteFadeSpeed);

        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignetteIntensity, Time.deltaTime * vignetteSpeed);
    }

    private void PlayerUISetUp()
    {
        UIManager.instance.OpenPanel(uiScreen);
        healthBar.maxValue = playerScript.MaxHealth;
        easeHealthBar.maxValue = playerScript.MaxHealth;

        healthBar.value = playerScript.CurrentHealth;
        easeHealthBar.value = playerScript.CurrentHealth;
    }

    private void UpdateHealthUI()
    {
        healthBar.maxValue = playerScript.MaxHealth;
        healthBar.value = playerScript.CurrentHealth;
        healthText.text = $"{playerScript.CurrentHealth}/{playerScript.MaxHealth}";
        TriggerHurtVignette();
    }
    private void TriggerHurtVignette()
    {
        Debug.Log("Trugger");
        targetVignetteIntensity = maxVignetteIntensity;
    }
}
