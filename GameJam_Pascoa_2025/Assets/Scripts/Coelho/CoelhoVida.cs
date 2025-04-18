using UnityEngine;

public class CoelhoVida : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int _vida, _vidaMax;
    [SerializeField] private int _morreu;
    [SerializeField] private SpriteRenderer _controladorSprite;
    [SerializeField] private Sprite _imgMorto;
    [SerializeField] CoelhoMovimentacao _controladorMovimentacao;
    [SerializeField] CoelhoFome _controladorFome;


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
    }

    private void Morreu()
    {
        _morreu = 1;
        _controladorSprite.sprite = _imgMorto;
        _controladorFome.enabled = false;
        _controladorMovimentacao.enabled = false;
        //Tratar a morte do jogador
    }

    public int getEstadoVida(){
        // 1 é morto, 0 é vivo
        return _morreu;
    }

    void Start()
    {
        _vida = _vidaMax;
        _morreu = 0;
    }
}
