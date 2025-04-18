using System.Collections;
using UnityEngine;

public class SpawnerSemente : MonoBehaviour
{
    [Header("Spawn de Semente")]
    [SerializeField] private GameObject _sementePrefab;
    [SerializeField] private float _randomPositionX;
    [SerializeField] private float _maxRangeSpawn;
    [SerializeField] private float _randomSpawnDelay;
    [SerializeField] private float _spawnDuration;
    [SerializeField] private bool _isSpawning;

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

    //Coroutine que spawna varias sementes com um delay aleatório
    private IEnumerator DelayedSpawn()
    {
        //inicia a contagem da duracao do spawn
        _spawnDuration = 4f;

        //enquanto a duracao do spawn for maior que 0 vai spawnar em intervalos aleatorios
        while(_spawnDuration >= 0)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(0.75f, 1.5f));
        }
        //quando a duracao do spawn acabar, sai do Coroutine e para de spawnar
        _randomSpawnDelay = Random.Range(6f, 20f);
        _isSpawning = false;
        yield break;
    }
        

    private void Start()
    {
        _isSpawning = false;
        //valor do limite da tela do player
        _maxRangeSpawn = 8f;
        //inicia os timers
        _randomSpawnDelay = Random.Range(6f, 20f);
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
        //se estiver spawnando, comeca a contagem da duracao do spawn
        else if (_isSpawning == true)
        {
            _spawnDuration -= Time.deltaTime;
        }
    }
}
