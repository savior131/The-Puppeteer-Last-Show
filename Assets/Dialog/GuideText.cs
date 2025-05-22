using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuideText : MonoBehaviour
{
    [SerializeField] List<string> text = new List<string>();
    [SerializeField] float timeBetweenChars = 2f;
    [SerializeField] TextMeshProUGUI displayedText;
    [SerializeField] string sceneToLoad = "startup scene";

    private void OnEnable()
    {
        displayedText.text = "";
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        foreach (string s in text)
        {
            displayedText.text = s;
            displayedText.DOFade(0.7f, timeBetweenChars);
            yield return new WaitForSeconds(timeBetweenChars);
            displayedText.DOFade(0, timeBetweenChars);
            yield return new WaitForSeconds(timeBetweenChars); // Wait for fade out
        }
        SceneFade.Instance.FadeToScene("startup scene");

        // Optional: disable GameObject
        // gameObject.SetActive(false);
    }
}
