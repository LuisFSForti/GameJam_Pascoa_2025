using UnityEngine;

public class CoelhoVida : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int _vida, _vidaMax;
    [SerializeField] private int _morreu;


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
        //Tratar a morte do jogador
    }

    public int getEstadoVida(){
        return _morreu;
    }

    void Start()
    {
        _vida = _vidaMax;
        _morreu = 0;
    }
}
