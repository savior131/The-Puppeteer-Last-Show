using UnityEngine;
using System.Collections;

public class ImpactFlash : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Material[] materials;
    private Color[] originalColors;
    private Coroutine flashRoutine;

    void Awake()
    {
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
        materials = targetRenderer.materials;
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].HasProperty("_Color"))
                originalColors[i] = materials[i].color;
        }
    }

    public void TriggerFlash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        foreach (var mat in materials)
        {
            if (mat.HasProperty("_Color"))
                mat.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].HasProperty("_Color"))
                materials[i].color = originalColors[i];
        }

        flashRoutine = null;
    }
}
