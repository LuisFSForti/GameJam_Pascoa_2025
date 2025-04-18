using UnityEngine;

public class SementeController : MonoBehaviour
{
    [Header("Movimentacao")]
    [SerializeField] private Rigidbody2D _rigidbody2d;
    [SerializeField] private float _velocidadeQueda;

    void Start()
    {
        //Pega o rigidbody2d da semente
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //faz ele cair
        _rigidbody2d.linearVelocityY = _velocidadeQueda * -1;

        //destroi a semente quando estiver abaixo da posicao -15 no eixo y
        if(transform.position.y <= -15f)
            Destroy(transform.parent.gameObject);
    }
}
