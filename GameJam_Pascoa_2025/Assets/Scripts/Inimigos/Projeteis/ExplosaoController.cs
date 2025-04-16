using UnityEngine;

public class ExplosaoController : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float _tempoDestruir;

    private void Start()
    {
        _tempoDestruir = 0.5f;
    }

    private void Update()
    {
        _tempoDestruir -= Time.deltaTime;

        if(_tempoDestruir <= 0 )
            Destroy(gameObject);
    }
}
