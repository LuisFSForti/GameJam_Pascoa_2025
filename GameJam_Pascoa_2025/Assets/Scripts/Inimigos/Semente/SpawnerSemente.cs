using System.Collections;
using UnityEngine;

public class SpawnerSemente : MonoBehaviour
{
    [Header("Spawn de Semente")]
    [SerializeField] private GameObject _sementePrefab;
    [SerializeField] private float _randomPositionX;
    [SerializeField] private float _maxRangeSpawn;
    [SerializeField] private float _randomSpawnDelay, _minSpawnDelay, _maxSpawnDelay;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private bool _isSpawning;

    //Pega a posicao do coelho
    private Transform _coelhoPosition;

    //funcao que spawna a semente em posicoes aleatorias pelo eixo x a partir da posicao do coelho
    public void Spawn()
    {
        _randomPositionX = Random.Range((_coelhoPosition.position.x - _maxRangeSpawn), (_coelhoPosition.position.x + _maxRangeSpawn));
        GameObject semente = Instantiate(_sementePrefab);
        semente.transform.position = new Vector2(_randomPositionX, (_coelhoPosition.position.y + 8f));
    }

    //Coroutine que spawna varias sementes com um delay aleatório
    private IEnumerator DelayedSpawn()
    {
        //inicia a contagem da duracao do spawn
        float t0 = Time.time;

        //enquanto a duracao do spawn for maior que 0 vai spawnar em intervalos aleatorios
        while(Time.time <= t0 + _spawnDuration)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(0.25f, 1f));
        }
        //quando a duracao do spawn acabar, sai do Coroutine e para de spawnar
        _randomSpawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        _isSpawning = false;
        yield break;
    }
        

    private void Start()
    {
        _isSpawning = false;
        //valor do limite da tela do player
        _maxRangeSpawn = 8f;
        //inicia os timers
        _randomSpawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);

        //Pega a posição do coelho
        _coelhoPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //diminui os timers
        _randomSpawnDelay -= Time.deltaTime;

        //toda vez que o timer zerar
        if (_randomSpawnDelay <= 0 && _isSpawning == false)
        {
            //comeca a spawnar
            _isSpawning = true;
            StartCoroutine(DelayedSpawn());
        }
    }
}
