using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private List<CinemachineCamera> virtualCameras;

    private Dictionary<string, CinemachineCamera> cameraMap = new();
    private Stack<string> cameraHistory = new();
    private string currentCamera;
    private Coroutine overrideRevertRoutine;

    private void Awake()
    {
        foreach (var cam in virtualCameras)
        {
            if (cam != null && !cameraMap.ContainsKey(cam.name))
            {
                cameraMap[cam.name] = cam;
            }
        }

        if (virtualCameras.Count > 0)
        {
            SetActiveCamera(virtualCameras[0].name);
        }
    }

    public void SetActiveCamera(string name)
    {
        if (!cameraMap.ContainsKey(name)) return;

        foreach (var kvp in cameraMap)
        {
            kvp.Value.Priority = kvp.Key == name ? 20 : 10;
        }

        if (!string.IsNullOrEmpty(currentCamera) && currentCamera != name)
        {
            cameraHistory.Push(currentCamera);
        }

        currentCamera = name;
    }

    public void OverrideCamera(string name, float autoRevertDelay = -1f)
    {
        if (!cameraMap.ContainsKey(name)) return;

        foreach (var kvp in cameraMap)
        {
            kvp.Value.Priority = kvp.Key == name ? 30 : 10;
        }

        if (autoRevertDelay > 0f)
        {
            if (overrideRevertRoutine != null)
                StopCoroutine(overrideRevertRoutine);

            overrideRevertRoutine = StartCoroutine(RevertAfterDelay(autoRevertDelay));
        }
    }

    public void ClearOverride()
    {
        if (overrideRevertRoutine != null)
        {
            StopCoroutine(overrideRevertRoutine);
            overrideRevertRoutine = null;
        }

        if (cameraHistory.Count > 0)
        {
            string previous = cameraHistory.Pop();
            SetActiveCamera(previous);
        }
        else
        {
            SetActiveCamera(currentCamera);
        }
    }

    private IEnumerator RevertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearOverride();
    }

    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        if (!cameraMap.ContainsKey(currentCamera)) return;

        var cam = cameraMap[currentCamera];
        var noise = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null) return;

        StartCoroutine(ShakeRoutine(noise, amplitude, frequency, duration));
    }

    private IEnumerator ShakeRoutine(CinemachineBasicMultiChannelPerlin noise, float amplitude, float frequency, float duration)
    {
        float timer = 0f;
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
}
