using UnityEngine;

public class BalaController : MonoBehaviour
{
    [Header("PosicaoCoelho")]
    [SerializeField] private Transform _coelhoPosition;

    [Header("Movimentacao")]
    [SerializeField] private float _velocidade;

    [SerializeField] private Rigidbody2D _rigidbody2d;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        _coelhoPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Vector2 direction = (_coelhoPosition.transform.position - transform.position).normalized;
        _rigidbody2d.linearVelocity = direction * _velocidade;  // Use velocity, not linearVelocity
    }

    public void setVelocidade(float _velocidadeDada){
        _velocidade = _velocidadeDada;
    }

    void OnBecameInvisible()
    {        
        Destroy(gameObject);
    }
}
