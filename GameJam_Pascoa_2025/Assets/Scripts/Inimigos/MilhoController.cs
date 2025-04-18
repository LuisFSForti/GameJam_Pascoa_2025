using UnityEngine;

public class MilhoController : MonoBehaviour
{
    [Header("PosicaoCoelho")]
    [SerializeField] private Transform _coelhoPosition;

    [Header("Movimentacao")]
    [SerializeField] private float _tiroVelocidade;
    [SerializeField] private float _tempoTiro;
    [SerializeField] private float _tempoAtirar;
    [SerializeField] private bool _estaAtirando;
    [SerializeField] private Rigidbody2D _rigidbody2d;
    [SerializeField] private float _range;

    [Header("Explosao")]
    [SerializeField] private GameObject _prefabTiro;
    //[SerializeField] private float _tempoAtirar;

    void Start()
    {
        //Localiza a posicao do coelho na fase
        _coelhoPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //garante que os timers comecem no 0 e que o milho nao comece atirando no player
        _tempoAtirar = _tempoTiro;
        _estaAtirando = false;
    }

    //funcao que faz o milho atirar ao "perceber" o player
    private void Tiro()
    {
        //fez o primeiro contato com o coelho
        if (_estaAtirando == false)
        {
            //pula para avisar que viu
            _rigidbody2d.AddForceY(3f, ForceMode2D.Impulse);

            _tempoAtirar = 0;
            _estaAtirando = true;
        }
        if (_tempoAtirar <= 0){
            _tempoAtirar = _tempoTiro;
            GameObject bala = Instantiate(_prefabTiro);
            BalaController balaController = bala.GetComponent<BalaController>();
            bala.transform.position = transform.position;
            balaController.setVelocidade(_tiroVelocidade);
            //A bala ira no proprio codigo se direcionar ao player
        }
        
        //comeca a atirar
    }

    private void DisableTiro()
    {
        _estaAtirando = false;
    }

    private void Update()
    {
        //==================================== Tiros ====================================

        //Diminui o timer
        _tempoAtirar -= Time.deltaTime;

        //mede a distancia entre o coelho e o milho
        float _distance = Mathf.Abs(transform.position.x - _coelhoPosition.transform.position.x);
        
        //checa se o milho esta na tela
        if(_distance <= _range)
        {
            //o milho atira ao "perceber" o player
            Tiro();
        }
        else
        {
            //O milho deixa de perceber o jogador, então para de atirar
            DisableTiro();
        }

    }
}
