using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [field: SerializeField] private Enemy _gameObject { get; set; }
    [field: SerializeField] private float _spawnerInterval { get; set; }
    [field: SerializeField] private int _enemyLimit { get; set; }
    [field: SerializeField] private float _rangeLimit { get; set; }
    public List<Enemy> ObjectList { get; set; }
    [field: SerializeField] private int _sumEnemyLimit;
    private int _totalEnemiesSpawned;


    // Start is called before the first frame update
    void Start()
    {
        ObjectList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");
        _totalEnemiesSpawned = 0;
    }


    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_spawnerInterval);
        if (ObjectList.Count < _enemyLimit && _totalEnemiesSpawned < _sumEnemyLimit)
        {
            float newXAxis = transform.position.x + Random.Range(-_rangeLimit, _rangeLimit);
            float newYAxis = transform.position.y + Random.Range(-_rangeLimit, _rangeLimit);
            Enemy newEnemy = Instantiate(_gameObject, new Vector2(newXAxis, newYAxis), Quaternion.identity);
            newEnemy.Spawner = this;
            ObjectList.Add(newEnemy);
            _totalEnemiesSpawned++;
            StartCoroutine("SpawnEnemy");
        }
    }
}
