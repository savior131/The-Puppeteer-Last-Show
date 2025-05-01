using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private BossHealth enemyHealth;

    private void Update()
    {
        UpdateValue(enemyHealth.CurrentHealth);
    }
    void UpdateValue(float value)
    {
        healthSlider.value = enemyHealth.CurrentHealth;
    }
}
