using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        if (IsSceneInBuild(sceneName))
        {
            triggered = true;
            SceneFade.Instance.FadeToScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' is not added to build settings.");
        }
    }

    private bool IsSceneInBuild(string name)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (sceneFileName.Equals(name))
                return true;
        }

        return false;
    }
}
