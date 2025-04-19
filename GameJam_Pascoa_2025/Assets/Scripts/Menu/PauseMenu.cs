using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject _pauseMenu;

    [Header("Jogador")]
    [SerializeField] private CoelhoVida _controladorVida;

    public void Pausar()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        if(_controladorVida.getEstadoVida() != 1)
        {
            Time.timeScale = 1;
            _pauseMenu.SetActive(false);
        }
    }

    //Recarrega a fase atual
    public void Reinicar()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && _controladorVida.getEstadoVida() != 1)
        {
            if (_pauseMenu.activeSelf == false)
            {
                Pausar();
            }
            else
            {
                Time.timeScale = 1;
                _pauseMenu.SetActive(false);
            }
        }
    }
}
