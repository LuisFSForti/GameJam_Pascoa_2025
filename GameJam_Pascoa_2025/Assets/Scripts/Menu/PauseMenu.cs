using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject _pauseMenu;

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu.activeSelf == false)
            {
                Time.timeScale = 0;
                _pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                _pauseMenu.SetActive(false);
            }
        }
    }
}
