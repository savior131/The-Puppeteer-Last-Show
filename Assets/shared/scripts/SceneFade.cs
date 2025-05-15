using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    public static SceneFade Instance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        yield return Fade(1, 0);
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(0, 1);
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return FadeIn();
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            yield return null;
        }
    }
}
