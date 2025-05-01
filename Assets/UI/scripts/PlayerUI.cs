using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Stamina playerStamina;

    private void Update()
    {
        UpdateValue(playerHealth.health);
    }
    void UpdateValue(float value)
    {
        healthSlider.value = playerHealth.health;
        staminaSlider.value = playerStamina.stamina;
    }
}
