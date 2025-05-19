using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnActivation : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void OnEnable()
    {
        if (IsSceneInBuild(sceneName))
        {
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
