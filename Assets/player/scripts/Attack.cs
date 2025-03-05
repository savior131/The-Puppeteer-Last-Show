using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxInputBuffer=3;
    private int comboStep;
    private bool canCombo;

    private int inputBuffer = -1;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (inputBuffer < maxInputBuffer)
            {
                inputBuffer++;
            }
            if (canCombo || comboStep == 0)
            {
                TryAttack();
            }
        }
        Debug.Log(inputBuffer);
    }

    public void TryAttack()
    {
        if (!canCombo && comboStep > 0) return;

        comboStep = (comboStep % 3) + 1;
        animator.ResetTrigger("attack" + ((comboStep == 1) ? 3 : comboStep - 1));
        animator.SetTrigger("attack" + comboStep);

        animator.CrossFade("attack" + comboStep, 0.1f, 0);
        canCombo = false;
    }

    public void EnableNextCombo()
    {
        canCombo = true;

        if (inputBuffer > 0)
        {
            inputBuffer--; 
            TryAttack();
        }
    }

    public void ResetCombo()
    {
        comboStep = 0;
        canCombo = false;
        inputBuffer = -1;
    }
}