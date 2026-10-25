using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Update()
    {
        easeHealthBar.value = Mathf.Lerp(easeHealthBar.value, playerScript.CurrentHealth, lerpSpeed);
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
    }

}
