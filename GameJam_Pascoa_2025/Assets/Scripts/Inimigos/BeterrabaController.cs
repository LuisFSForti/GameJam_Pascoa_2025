using UnityEngine;

public class BeterrabaController : MonoBehaviour
{
    [Header("PosicaoCoelho")]
    [SerializeField] private Transform _coelhoPosition;

    [Header("Movimentacao")]
    [SerializeField] private float _velocidade;
    [SerializeField] private float _distance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _tempoPulo;
    [SerializeField] private bool _estaPerseguindo;
    [SerializeField] private Rigidbody2D _rigidbody2d;

    [Header("Explosao")]
    [SerializeField] private GameObject _prefabExplosao;
    [SerializeField] private float _tempoExplodir;

    void Start()
    {
        //Localiza a posicao do coelho na fase
        _coelhoPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //garante que os timers comecem no 0 e que a beterraba nao comece perseguindo o player
        _tempoExplodir = 0f;
        _tempoPulo = 0f;
        _estaPerseguindo = false;
    }

    //funcao que faz a beterraba pular ao "perceber" o player
    private void Pulo()
    {
        //fez o primeiro contato com o coelho
        if (_estaPerseguindo == false)
        {
            _rigidbody2d.AddForceY(3f, ForceMode2D.Impulse);
            _tempoPulo = 1f;
            _tempoExplodir = 8f;
        }
        //comeca a perseguir o coelho
        _estaPerseguindo = true;
    }

    private void Update()
    {
        //==================================== Movimentacao ====================================

        //Diminui os timers
        _tempoPulo -= Time.deltaTime;
        _tempoExplodir -= Time.deltaTime;

        //mede a distancia entre o coelho e a beterraba
        _distance = Mathf.Abs(transform.position.x - _coelhoPosition.transform.position.x);
        
        //checa se a beterraba entrou na tela do player
        if(_distance <= _maxDistance)
        {
            //a beterraba pula ao "perceber" o player
            Pulo();
        }

        //se a beterraba estiver a esquerda do coelho
        if (transform.position.x < _coelhoPosition.transform.position.x && _estaPerseguindo == true && _tempoPulo <= 0)
            _rigidbody2d.linearVelocityX = _velocidade;
        //se a beterraba estiver a direita do coelho
        else if (transform.position.x > _coelhoPosition.transform.position.x && _estaPerseguindo == true && _tempoPulo <= 0)
            _rigidbody2d.linearVelocityX = _velocidade * -1;

        //========================================= Explosao ====================================

        //se o tempo de explodir acabar ou chegar perto do player, a baterraba explode
        if (_tempoExplodir <= 0f && _estaPerseguindo == true || _distance <= 1.1f)
        {
            GameObject explosao = Instantiate(_prefabExplosao);
            explosao.transform.position = transform.position;
            Destroy(transform.parent.gameObject);
        }
    }
}
