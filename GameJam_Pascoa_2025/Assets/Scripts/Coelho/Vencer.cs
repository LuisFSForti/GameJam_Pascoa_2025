using UnityEngine;
using UnityEngine.SceneManagement;

public class Vencer : MonoBehaviour
{
    [SerializeField] private Vector2 _posicaoVenceu;

    void Update()
    {
        if(_posicaoVenceu.x <= transform.position.x && _posicaoVenceu.y >= transform.position.y)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
