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
    public bool CanRespawn { get; set; }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        ObjectList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");
        _totalEnemiesSpawned = 0;
        CanRespawn = true;
    }


    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_spawnerInterval);
        if (_totalEnemiesSpawned < _sumEnemyLimit)
        {
            if (ObjectList.Count < _enemyLimit)
            {
                Debug.Log("ObjectList.Count < _enemyLimit");
                float newXAxis = transform.position.x + Random.Range(-_rangeLimit, _rangeLimit);
                float newYAxis = transform.position.y + Random.Range(-_rangeLimit, _rangeLimit);
                Enemy newEnemy = Instantiate(_gameObject, new Vector2(newXAxis, newYAxis), Quaternion.identity);
                newEnemy.Spawner = this;

                newEnemy.Power += GameController.GameControllerInstance.Difficult;
                newEnemy.LifeCap += GameController.GameControllerInstance.Difficult;
                newEnemy.Life += GameController.GameControllerInstance.Difficult;
            
                ObjectList.Add(newEnemy);
                _totalEnemiesSpawned++;
                

            }
            
        }
        StartCoroutine("SpawnEnemy");

        if (_totalEnemiesSpawned == _sumEnemyLimit && ObjectList.Count == 0)
        {
            CanRespawn = false;
        }

    }
}
