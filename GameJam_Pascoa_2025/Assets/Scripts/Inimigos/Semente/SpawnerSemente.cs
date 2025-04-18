using UnityEngine;

public class SpawnerSemente : MonoBehaviour
{
    [Header("Spawn de Semente")]
    [SerializeField] private GameObject _sementePrefab;
    [SerializeField] private float _randomPositionX;
    [SerializeField] private float _maxRangeSpawn;
    [SerializeField] private float _randomSpawnDelay;

    //Pega a posicao do coelho
    private Vector2 _coelhoPosition;

    //funcao que spawna a semente em posicoes aleatorias pelo eixo x a partir da posicao do coelho
    public void Spawn()
    {
        _coelhoPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        _randomPositionX = Random.Range((_coelhoPosition.x - _maxRangeSpawn), (_coelhoPosition.x + _maxRangeSpawn));
        GameObject semente = Instantiate(_sementePrefab);
        semente.transform.position = new Vector2(_randomPositionX, (_coelhoPosition.y + 8f));
    }

    private void Start()
    {
        //valor do limite da tela do player
        _maxRangeSpawn = 8f;
        //inicia o timer
        _randomSpawnDelay = Random.Range(3f, 20f);
    }
    private void Update()
    {
        //diminui o timer
        _randomSpawnDelay -= Time.deltaTime;

        //toda vez que o timer zerar, spawna 1 semente e reinicia o timer de forma aleatoria
        if (_randomSpawnDelay <= 0)
        {
            Spawn();
            _randomSpawnDelay = Random.Range(3f, 20f);
        }

    }
}
