using UnityEngine;

public class BatataController : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private float _velocidade;

    [Header("Controle")]
    [SerializeField] private float _distCheck;
    [SerializeField] private Transform _limiteE, _limiteD;
    [SerializeField] private Rigidbody2D _corpo;


    void Update()
    {
        float dir = transform.localScale.x >= 0 ? 1 : -1;

        if (dir > 0)
        {
            if (transform.position.x >= _limiteD.position.x)
                dir = -1;
        }
        else
        {
            if (transform.position.x <= _limiteE.position.x)
                dir = 1;
        }

        Debug.Log(Physics2D.Raycast(transform.position, dir * transform.right, _distCheck + Mathf.Abs(transform.localScale.x / 2)).collider);

        if (Physics2D.Raycast(transform.position, dir * transform.right, _distCheck + Mathf.Abs(transform.localScale.x/2)))
            dir = transform.localScale.x > 0 ? -1 : 1;

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

        _corpo.linearVelocityX = _velocidade * dir;
    }
}
