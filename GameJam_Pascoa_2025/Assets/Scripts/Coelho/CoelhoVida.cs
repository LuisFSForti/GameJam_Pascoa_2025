using UnityEngine;

public class CoelhoVida : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int _vida, _vidaMax;

    //Para alterar a vida do jogador
    public void MudarVida(int valor)
    {
        _vida += valor;

        if(_vida > _vidaMax)
            _vida = _vidaMax;

        if(_vida < 0)
        {
            _vida = 0;
            Morreu();
        }
    }

    private void Morreu()
    {
        //Tratar a morte do jogador
    }

    void Start()
    {
        _vida = _vidaMax;
    }
}
