using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject _pauseMenu;

    [Header("Jogador")]
    [SerializeField] private CoelhoVida _controladorVida;

    [Header("Audio")]
    [SerializeField] private AudioSource _musicaFundo;

    public void Pausar()
    {
        _musicaFundo.Pause();
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        if(_controladorVida.getEstadoVida() != 1)
        {
            _musicaFundo.Play();
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu.activeSelf == false)
            {
                Pausar();
            }
            else if(_controladorVida.getEstadoVida() != 1)
            {
                Time.timeScale = 1;
                _pauseMenu.SetActive(false);
            }
        }
    }
}
