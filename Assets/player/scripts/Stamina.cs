using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float regenDelay = 2f;

    private float currentStamina;
    private float lastDrainTime;

    public bool IsDrained => currentStamina <= 0;
    public float stamina => currentStamina;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if (Time.time - lastDrainTime > regenDelay)
        {
            RegenerateStamina(Time.deltaTime);
        }
    }

    public void gradualDraining(bool draining, float deltaTime)
    {
        if (draining && !IsDrained)
        {
            DrainStamina(deltaTime);
            lastDrainTime = Time.time;
        }
    }

    public void DrainChunk(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0;
        lastDrainTime = Time.time;
    }

    void DrainStamina(float deltaTime)
    {
        currentStamina -= staminaDrainRate * deltaTime;
        if (currentStamina < 0) currentStamina = 0;
    }

    void RegenerateStamina(float deltaTime)
    {
        currentStamina += staminaRegenRate * deltaTime;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
    }
}
