using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Stamina playerStamina;
    private void Awake()
    {
        UpdateValue(playerHealth.health);
    }
    private void Update()
    {
        UpdateValue(playerHealth.health);
    }
    void UpdateValue(float value)
    {
        healthSlider.value = playerHealth.health;
        healthText.text = value.ToString("0.00");
        staminaSlider.value = playerStamina.stamina;
    }
}
