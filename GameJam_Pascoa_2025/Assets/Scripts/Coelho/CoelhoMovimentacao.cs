using System.ComponentModel;
using UnityEngine;

public class CoelhoMovimentacao : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _controladorSprite;
    [SerializeField] private Sprite _imgNormal, _imgFaminto, _imgGordo, _imgBombado;
    
    [Header("Movimenta��o-Gen�rica")]
    [SerializeField] private float _distChao, _coyoteTime, _tempoNoAr;

    [Header("Movimenta��o-Normal")]
    [SerializeField] private float _velocidadeNormal, _forcaPuloAndarNormal, _forcaPuloMaxNormal;

    [Header("Movimenta��o-Faminto")]
    [SerializeField] private float _velocidadeFaminto, _forcaPuloAndarFaminto, _forcaPuloMaxFaminto;

    [Header("Movimenta��o-Gordo")]
    [SerializeField] private float _velocidadeGordo, _forcaPuloAndarGordo, _forcaPuloMaxGordo, _desaceleracaoDestruicao;

    [Header("Movimenta��o-Bombado")]
    [SerializeField] private float _velocidadeBombado, _forcaPuloAndarBombado, _forcaPuloMaxBombado;

    [Header("Comida")]
    [SerializeField] CoelhoFome _controladorFome;
    [SerializeField] private float _tempoComer;

    [Header("Controle")]
    [SerializeField] private Rigidbody2D _corpo;
    [SerializeField] private float _estaParalizado;
    [SerializeField] private LayerMask _layersChao, _layersDestrutiveis;
    [SerializeField] private bool _estaNoChao;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Espinafre")
        {
            _controladorFome.Comer(1f, 'B');

            _corpo.linearVelocity = Vector2.zero;
            _estaParalizado = _tempoComer;

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //https://discussions.unity.com/t/check-if-layer-is-in-layermask/16007
        if ((1<<collision.collider.gameObject.layer | _layersDestrutiveis) == _layersDestrutiveis)
        {
            char estado = _controladorFome.GetEstado();
            if(estado == 'G' || estado == 'B')
            {
                _corpo.linearVelocity /= _desaceleracaoDestruicao;
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private void Start()
    {
        _estaParalizado = 0f;
    }

    //A cada frame
    void Update()
    {
        Sprite img = _imgNormal;
        switch (_controladorFome.GetEstado())
        {
            case 'F':
                img = _imgFaminto;
                transform.localScale = new Vector3(1, 0.5f, 0);
                break;

            case 'B':
                transform.localScale = new Vector3(2, 1.5f, 0);
                img = _imgBombado;
                break;

            case 'G':
                img = _imgGordo;
                transform.localScale = new Vector3(2, 2, 0);
                break;

            default:
            case 'N':
                transform.localScale = Vector3.one;
                break;
        }

        _controladorSprite.sprite = img;

        //Se o jogador estiver sob um stun (quando ele recebe dano, por exemplo)
        if (_estaParalizado > 0)
        {
            //Diminui o tempo de stun
            _estaParalizado -= Time.deltaTime;
            //N�o avan�a para a se��o de controle do jogador
            return;
        }

        //=======================================================   Se��o de controle do jogador    =====================================================

        //Verifica se ele est� tocando o ch�o
        //Cria uma "caixa" abaixo dele, retornando se tocou algo com do layer "chao" ou n�o
        _estaNoChao = Physics2D.BoxCast(transform.position, new Vector2(transform.localScale.x, _distChao), 0, -transform.up, transform.localScale.y / 2, _layersChao);


        //=======================================================               Comer               ======================================================

        //Se estiver no ch�o e jogador estiver pedindo pra comer
        if(_estaNoChao && Input.GetKey(KeyCode.C))
        {
            //Come uma cenoura, falando que est� tentando ficar gordo
            _controladorFome.Comer(0.5f, 'G');

            //Paraliza o coelho
            _corpo.linearVelocity = Vector2.zero;
            _estaParalizado = _tempoComer;

            //Termina de analisar os comandos
            return;
        }

        //=======================================================           Movimento               ======================================================


        //Atualiza o tempo no ar
        //Se o jogador estiver no ch�o, vai pra 0
        //Se estiver no ar, aumenta
        _tempoNoAr = (_tempoNoAr + Time.deltaTime) * (_estaNoChao ? 0 : 1);

        //Qual ser� a for�a aplicada em Y
        float forcaY = 0f, forcaPuloAndar = 0f, forcaPuloMax = 0f, velocidade = 0f;

        switch (_controladorFome.GetEstado())
        {
            case 'F':
                velocidade = _velocidadeFaminto;
                forcaPuloAndar = _forcaPuloAndarFaminto;
                forcaPuloMax = _forcaPuloMaxFaminto;
                break;

            case 'B':
                velocidade = _velocidadeBombado;
                forcaPuloAndar = _forcaPuloAndarBombado;
                forcaPuloMax = _forcaPuloMaxBombado;
                break;

            case 'G':
                velocidade = _velocidadeGordo;
                forcaPuloAndar = _forcaPuloAndarGordo;
                forcaPuloMax = _forcaPuloMaxGordo;
                break;

            default:
            case 'N':
                velocidade = _velocidadeNormal;
                forcaPuloAndar = _forcaPuloAndarNormal;
                forcaPuloMax = _forcaPuloMaxNormal;
                break;
        }

        //Se ele n�o est� no ar a tempo demais sem ter pulado (https://evolvers.com.br/como-criar-coyote-time/#:~:text=Resumo,que%20%C3%A9%20o%20Coyote%20Time)
        //e se ele j� n�o estiver subindo
        if (_tempoNoAr <= _coyoteTime)
        {
            //Se o jogador apertou espa�o
            if (Input.GetKey(KeyCode.Space))
            {
                //Usa a velocidade de pulo mais alto
                forcaY = forcaPuloMax;

                //Impede o jogador de pular novamente
                _tempoNoAr = _coyoteTime + 0.1f;
            }
            //Se n�o apertou e estiver no ch�o
            else if(_estaNoChao)
            {
                //Usa a velocidade de pulo de andar normal se o jogador estiver andando
                forcaY = forcaPuloAndar * (Input.GetAxisRaw("Horizontal") != 0 ? 1 : 0);
            }
        }

        //Define qual � a velocidade em X
        _corpo.linearVelocityX = velocidade * Input.GetAxisRaw("Horizontal");
        
        //Empurra o jogador para cima
        if(forcaY > 0)
        {
            //Zera a velocidade em Y pra sobrescrever com o pulo
            _corpo.linearVelocityY = 0;
            _corpo.AddForceY(forcaY, ForceMode2D.Impulse);
        }

        //Se o jogador soltar o espa�o durante o pulo, ele n�o quer ir mais alto
        if (Input.GetKeyUp(KeyCode.Space) && _corpo.linearVelocityY > 0)
        {
            //Corta a velocidade vertical pela metade
            _corpo.linearVelocityY /= 2;
        }
    }
}
