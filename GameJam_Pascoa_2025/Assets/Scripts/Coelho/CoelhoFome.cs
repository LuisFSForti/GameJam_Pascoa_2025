using UnityEngine;
using UnityEngine.UI;

public class CoelhoFome : MonoBehaviour
{
    [Header("Comida")]
    [SerializeField] private float _fome, _perdaFome, _limiteInferior, _limiteSuperior;
    [SerializeField] private char _estado;
    /*
     Estados:
    'F' - faminto
    'N' - normal
    'G' - gordo
    'B' - bombado

    Como s� 'G' e 'B' dividem uma mesma faixa de fome, ent�o _estado s� precisa guardar estes valores
     */

    [Header("UI")]
    [SerializeField] private float _tamanhoMax;
    [SerializeField] private RawImage _progresso;

    [Header("Controle")]
    [SerializeField] CoelhoVida _controladorVida;

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
        {
            _fome = 0;
            //Mata o jogador de fome
            _controladorVida.MudarVida(-1000000);
        }
        else if (_fome > 1)
            _fome = 1;

        //Para qual estado ele est� indo
        _estado = tipo;
    }

    //Retorna o estado do coelho
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
        //Deixa ele com o m�ximo poss�vel de fome estando na forma normal
        _fome = _limiteSuperior;
        //Estado neutro, n�o importa o valor aqui pois ele ser� alterado
        _estado = 'N';
    }

    void Update()
    {
        //Perde fome, mantendo o estado atual
        Comer(-_perdaFome * Time.deltaTime, _estado);

        //Atualiza a barra de fome na UI
        _progresso.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _fome * _tamanhoMax);
    }
}
