using System.Collections;
using TMPro;
using UnityEngine;

public class CoelhoVida : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int _vida, _vidaMax;
    [SerializeField] private int _morreu;

    [Header("Controle")]
    [SerializeField] private SpriteRenderer _controladorSprite;
    [SerializeField] private Sprite _imgMorto;

    [SerializeField] private Rigidbody2D _corpo;
    [SerializeField] private CoelhoMovimentacao _controladorMovimentacao;
    [SerializeField] private CoelhoFome _controladorFome;

    [SerializeField] private float _tempoMorrer;
    [SerializeField] private PauseMenu _pauseMenu;

    [Header("UI")]
    [SerializeField] private TMP_Text _textoVidas;

    [Header("Audio")]
    [SerializeField] private AudioSource _musicaFundo, _fonte;
    [SerializeField] private AudioClip _choro;


    //Para alterar a vida do jogador
    public void MudarVida(int valor)
    {
        _vida += valor;

        if(_vida > _vidaMax)
            _vida = _vidaMax;

        if(_vida <= 0)
        {
            _vida = 0;
            Morreu();
        }

        _textoVidas.text = "x" + _vida.ToString();
    }

    private void Morreu()
    {
        _corpo.linearVelocityX = 0;

        _controladorSprite.sprite = _imgMorto;
        transform.localScale = Vector3.one;

        _controladorFome.enabled = false;
        _controladorMovimentacao.enabled = false;

        if (_morreu == 0)
            StartCoroutine(EsperarAnimacao());

        _morreu = 1;
    }

    private IEnumerator EsperarAnimacao()
    {
        _musicaFundo.Stop();

        yield return new WaitForEndOfFrame();

        _fonte.Stop();
        _fonte.clip = _choro;
        _fonte.Play();

        yield return new WaitForSeconds(_tempoMorrer);
        _pauseMenu.Pausar();
    }

    public int getEstadoVida(){
        // 1 é morto, 0 é vivo
        return _morreu;
    }

    void Start()
    {
        _vida = _vidaMax;
        _morreu = 0;
        MudarVida(0); //Para atualizar o texto de vida
    }
}
