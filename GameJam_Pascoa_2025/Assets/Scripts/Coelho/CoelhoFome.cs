using UnityEngine;
using UnityEngine.UI;

public class CoelhoFome : MonoBehaviour
{
    [Header("Comida")]
    [SerializeField] private float _fome, _perdaFome, _limiteInferior, _limiteSuperior;
    [SerializeField] private char _estado;

    [Header("UI")]
    [SerializeField] private float _tamanhoMax;
    [SerializeField] private RawImage _progresso;

    //C�digos externos podem acessar a fome, mas n�o podem alter�-lo
    public float Fome
    {
        get { return _fome; }
    }

    //Para alterar a fome do coelho
    public void Comer(float valor, char tipo)
    {
        _fome += valor;

        if (_fome < 0)
            _fome = 0;
        else if (_fome > 1)
            _fome = 1;

        _estado = tipo;
    }

    public char GetEstado()
    {
        if (_fome <= _limiteInferior)
            return 'F'; //Coelho est� faminto

        if (_fome < _limiteSuperior)
            return 'N'; //Coelho est� normal
        else
            return _estado; //Coelho est� gordo ou bombado
    }

    private void Start()
    {
        _fome = _limiteSuperior;
        _estado = 'N';
    }

    //A cada frame
    void Update()
    {
        _fome -=_perdaFome * Time.deltaTime;

        _progresso.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _fome * _tamanhoMax);
    }
}
