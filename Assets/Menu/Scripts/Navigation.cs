using UnityEngine;
using UnityEngine.SceneManagement;
public class Navigation : MonoBehaviour
{
    
    public void Play()
    {
        SceneFade.Instance.FadeToScene("intro");
    }
    public void exit()
    {
        Application.Quit();
    }
}
