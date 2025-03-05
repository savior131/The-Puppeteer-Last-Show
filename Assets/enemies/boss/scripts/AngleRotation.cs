using UnityEngine;

public class AngleRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool invertRotation = false;
    [SerializeField] private new Transform transform;

    private Quaternion targetRotation;
    private bool rotating;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                rotating = false;
            }
        }
    }

    public void SetRotation(float angle)
    {
        if (invertRotation) angle = angle+180;
        angle = Mathf.Repeat(angle, 360f);
        targetRotation = Quaternion.Euler(0, angle, 0);
        rotating = true;
    }
}
