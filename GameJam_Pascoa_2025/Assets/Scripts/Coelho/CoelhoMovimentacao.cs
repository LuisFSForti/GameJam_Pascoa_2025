using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.UIElements;
using UnityEngine;

public class CoelhoMovimentacao : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _controladorSprite;
    [SerializeField] private Sprite _imgNormal, _imgFaminto, _imgGordo, _imgBombado;
    
    [Header("Movimentação-Genérica")]
    [SerializeField] private float _distChao, _coyoteTime, _tempoNoAr;

    [Header("Movimentação-Normal")]
    [SerializeField] private float _velocidadeNormal, _forcaPuloAndarNormal, _forcaPuloMaxNormal;

    [Header("Movimentação-Faminto")]
    [SerializeField] private float _velocidadeFaminto, _forcaPuloAndarFaminto, _forcaPuloMaxFaminto;

    [Header("Movimentação-Gordo")]
    [SerializeField] private float _velocidadeGordo, _forcaPuloAndarGordo, _forcaPuloMaxGordo, _desaceleracaoDestruicao;

    [Header("Movimentação-Bombado")]
    [SerializeField] private float _velocidadeBombado, _forcaPuloAndarBombado, _forcaPuloMaxBombado;

    [Header("Comida")]
    [SerializeField] CoelhoFome _controladorFome;
    [SerializeField] private float _tempoComer;

    [Header("Vida")]
    [SerializeField] CoelhoVida _controladorVida;
    [SerializeField] private float _tempoVida, _forcaImpacto;

    [Header("Controle")]
    [SerializeField] private Rigidbody2D _corpo;
    [SerializeField] private float _estaParalizado;
    [SerializeField] private LayerMask _layersChao, _layersDestrutiveis;
    [SerializeField] private List<string> _tagCausadorasDeDano, _tagsCausadorasDeDanoIndestrutivies; //Não existe uma classe pré-pronta pra tratar as tags
    [SerializeField] private bool _estaNoChao;

    //Quando colidir com um objeto com collider do tipo trigger
    /*
     In Unity, a trigger is a specialized collider that detects when other colliders enter, 
     exit, or stay within its space, without actually causing a physical collision. This 
     allows you to create events and logic based on an object's proximity to the trigger 
     area, rather than a direct physical impact. 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se for uma espinafre
        if (collision.gameObject.tag == "Espinafre")
        {
            //Coloca a fome no máximo e define que está ficando bombado
            _controladorFome.Comer(1f, 'B');

            //Para o jogador e o paraliza para comer
            _corpo.linearVelocity = Vector2.zero;
            _estaParalizado = _tempoComer;

            //Destroi a espinafre
            Destroy(collision.gameObject);
        }
    }

    //Quando colidir com um objeto com collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se for algum objeto que cause dano
        if(_tagCausadorasDeDano.Contains(collision.collider.tag))
        {
            //Se não for indestrutível
            if(!_tagsCausadorasDeDanoIndestrutivies.Contains(collision.collider.tag))
            {
                //Dependendo do estado do jogador
                switch (_controladorFome.GetEstado())
                {
                    //Se faminto
                    case 'F':
                        //Destrói o container do objeto
                        Destroy(collision.collider.gameObject.transform.parent.gameObject);

                        //Come ele
                        _controladorFome.Comer(0.2f, 'G');

                        //Paraliza o jogador por um curto intervalo
                        _estaParalizado = _tempoComer / 3;
                        _corpo.linearVelocity = Vector2.zero;

                        //Sai da função
                        return;

                    //Se for o gordo ou o bombado
                    case 'G':
                    case 'B':
                        //Destrói o container do objeto
                        Destroy(collision.collider.gameObject.transform.parent.gameObject);

                        //Sai da função
                        return;

                    //Se estiver no estado normal, então sai do switch
                    default:
                        break;
                }
            }
            //Se for indestrutível
            else
            {
                //Se estiver bombado
                if(_controladorFome.GetEstado() == 'B')
                {
                    //Destrói o container do objeto
                    Destroy(collision.collider.gameObject.transform.parent.gameObject);

                    //Sai da função
                    return;
                }
            }

            //Se não destruiu o objeto

            //Perde vida
            _controladorVida.MudarVida(-1);

            //Paraliza o jogador
            _estaParalizado = _tempoVida;
            _corpo.linearVelocity = Vector2.zero;

            //Empurra ele na direção oposta à fonte de dano
            _corpo.AddForce(new Vector2((collision.collider.transform.position.x > transform.position.x ? -1 : 1) * _forcaImpacto, 1), ForceMode2D.Impulse);

            //Sai da função
            return;
        }

        //Se não for um objeto que causa dnao

        //Verifica se é uma estrutura destrutível
        //https://discussions.unity.com/t/check-if-layer-is-in-layermask/16007
        if ((1<<collision.collider.gameObject.layer | _layersDestrutiveis) == _layersDestrutiveis)
        {
            //Se for, verifica se está gordo ou bombado
            char estado = _controladorFome.GetEstado();
            if(estado == 'G' || estado == 'B')
            {
                //Se sim, desacelera o jogador e destrói a estrutura
                _corpo.linearVelocity /= _desaceleracaoDestruicao;
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private void Start()
    {
        _estaParalizado = 0f;
    }

    void Update()
    {
        //Atualiza o sprite e o tamanho de acordo com o estado
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
                transform.localScale = Vector3.one;
                break;
        }
        _controladorSprite.sprite = img;

        //Se o jogador estiver sob um stun (quando ele recebe dano, por exemplo)
        if (_estaParalizado > 0)
        {
            //Diminui o tempo de stun
            _estaParalizado -= Time.deltaTime;
            //Não avança para a seção de controle do jogador
            return;
        }

        //=======================================================   Seção de controle do jogador    =====================================================

        //Verifica se ele está tocando o chão
        //Cria uma "caixa" abaixo dele, retornando se tocou algo com do layer "chao" ou não
        _estaNoChao = Physics2D.BoxCast(transform.position, new Vector2(transform.localScale.x, _distChao), 0, -transform.up, transform.localScale.y / 2, _layersChao);


        //===============================               Comer               ====================================

        //Se estiver no chão e jogador estiver pedindo pra comer
        if(_estaNoChao && Input.GetKey(KeyCode.C))
        {
            //Come uma cenoura, falando que está tentando ficar gordo
            _controladorFome.Comer(0.5f, 'G');

            //Paraliza o coelho
            _corpo.linearVelocity = Vector2.zero;
            _estaParalizado = _tempoComer;

            //Sai da função
            return;
        }

        //===============================          Movimento               =====================================


        //Atualiza o tempo no ar
        //Se o jogador estiver no chão, vai pra 0
        //Se estiver no ar, aumenta
        _tempoNoAr = (_tempoNoAr + Time.deltaTime) * (_estaNoChao ? 0 : 1);

        //Quais serão os valores utilizados
        float forcaY = 0f, forcaPuloAndar = 0f, forcaPuloMax = 0f, velocidade = 0f;

        //Define qual velocidade e força usar de acordo com o estado do jogador
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
                velocidade = _velocidadeNormal;
                forcaPuloAndar = _forcaPuloAndarNormal;
                forcaPuloMax = _forcaPuloMaxNormal;
                break;
        }

        //Se ele não está no ar a tempo demais sem ter pulado (https://evolvers.com.br/como-criar-coyote-time/#:~:text=Resumo,que%20%C3%A9%20o%20Coyote%20Time)
        if (_tempoNoAr <= _coyoteTime)
        {
            //Se o jogador apertou espaço
            if (Input.GetKey(KeyCode.Space))
            {
                //Usa a velocidade de pulo mais alto
                forcaY = forcaPuloMax;

                //Impede o jogador de pular novamente
                _tempoNoAr = _coyoteTime + 0.1f;
            }
            //Se não apertou e estiver no chão
            else if(_estaNoChao)
            {
                //Usa a velocidade de pulo de andar normal se o jogador estiver andando
                if(Input.GetAxisRaw("Horizontal") != 0)
                    forcaY = forcaPuloAndar;
            }
        }

        //Define qual é a velocidade em X
        _corpo.linearVelocityX = velocidade * Input.GetAxisRaw("Horizontal");
        
        //Se o jogador estiver fazendo força para cima
        if(forcaY > 0)
        {
            //Zera a velocidade em Y pra sobrescrever com o pulo
            _corpo.linearVelocityY = 0;
            //Empurra o jogador com a força passada na forma de impulso
            _corpo.AddForceY(forcaY, ForceMode2D.Impulse);
        }

        //Se o jogador soltar o espaço durante o pulo, ele não quer ir mais alto
        if (Input.GetKeyUp(KeyCode.Space) && _corpo.linearVelocityY > 0)
        {
            //Corta a velocidade vertical pela metade
            _corpo.linearVelocityY /= 2;
        }
    }
}
