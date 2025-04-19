using UnityEngine;

public class BatataController : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private float _velocidade;

    [Header("Controle")]
    [SerializeField] private Transform _limiteE, _limiteD;
    [SerializeField] private Rigidbody2D _corpo;

    //Quando ele colidir com outro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se o objeto não estiver muito próximo à base da batata, ou seja, se não for o chão
        //Então funciona com paredes e outras criaturas
        if(collision.collider.transform.position.y > transform.position.y - transform.localScale.y/2 || collision.collider.tag == "Inimigo")
        {
            //Inverte a direção do movimento
            _velocidade *= -1;
        }
    }

    void Update()
    {
        //Se estiver indo pra direita
        if (_velocidade > 0)
        {
            //Verifica se não passou do limite à direita
            if (transform.position.x >= _limiteD.position.x)
                //Se sim, inverte a direção
                _velocidade *= -1;
        }
        //Se for para a esquerda
        else
        {
            //Verifica se não passou do limite à esquerda
            if (transform.position.x <= _limiteE.position.x)
                //Se sim, inverte a direção
                _velocidade *= -1;
        }

        //Se estiver indo para a direita
        if (_velocidade >= 0)
            //Orienta encarando a direita
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //Se for para a esquerda
        else
            //Orienta encarando a esquerda
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        //Define a velocidade de movimento
        _corpo.linearVelocityX = _velocidade;
    }
}
