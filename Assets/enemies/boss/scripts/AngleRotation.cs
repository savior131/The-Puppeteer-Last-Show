using UnityEngine;

public class AngleRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool invertRotation = false;
    [SerializeField] private Transform yAxisTarget;
    [SerializeField] private Transform xAxisTarget;

    private Quaternion yTargetRotation;
    private Quaternion xTargetRotation;

    private bool rotatingY;
    private bool rotatingX;

    void Start()
    {
        if (yAxisTarget == null) yAxisTarget = transform;
        if (xAxisTarget == null) xAxisTarget = transform;

        yTargetRotation = yAxisTarget.rotation;
        xTargetRotation = xAxisTarget.rotation;
    }

    void Update()
    {
        if (rotatingX)
        {
            xAxisTarget.localRotation = Quaternion.Slerp(xAxisTarget.localRotation, xTargetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(xAxisTarget.localRotation, xTargetRotation) < 0.1f)
            {
                xAxisTarget.localRotation = xTargetRotation;
                rotatingX = false;
            }
        }
        if (rotatingY)
        {
            yAxisTarget.rotation = Quaternion.Slerp(yAxisTarget.rotation, yTargetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(yAxisTarget.rotation, yTargetRotation) < 0.1f)
            {
                yAxisTarget.rotation = yTargetRotation;
                rotatingY = false;
            }
        }


    }
    public void SetYRotation(float angleY)
    {
        
        angleY = Mathf.Repeat(angleY, 360f);

        Vector3 currentEuler = yAxisTarget.rotation.eulerAngles;
        yTargetRotation = Quaternion.Euler(currentEuler.x, angleY, currentEuler.z);
        rotatingY = true;
    }

    public void SetXRotation(float angleX)
    {
        angleX = Mathf.Repeat(angleX, 360f);

        Vector3 currentEuler = xAxisTarget.localRotation.eulerAngles;
        xTargetRotation = Quaternion.Euler(angleX, currentEuler.y, currentEuler.z);
        rotatingX = true;
    }

}
