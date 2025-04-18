using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
