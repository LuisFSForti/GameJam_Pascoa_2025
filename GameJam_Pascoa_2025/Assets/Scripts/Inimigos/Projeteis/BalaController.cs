using UnityEngine;

public class BalaController : MonoBehaviour
{
    [Header("PosicaoCoelho")]
    [SerializeField] private Rigidbody2D _coelhoPosition;

    [Header("Movimentacao")]
    [SerializeField] private float _velocidade;
    [SerializeField] private Rigidbody2D _rigidbody2d;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        _coelhoPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        Vector2 direction = _coelhoPosition.transform.position + new Vector3(_coelhoPosition.linearVelocityX, _coelhoPosition.linearVelocityY, 0) * Random.Range(0, 2) - transform.position;
        _rigidbody2d.linearVelocity = direction.normalized * _velocidade;  // Use velocity, not linearVelocity
    }

    public void setVelocidade(float _velocidadeDada){
        _velocidade = _velocidadeDada;
    }

    void OnBecameInvisible()
    {        
        Destroy(gameObject);
    }
}
