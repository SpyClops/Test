using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public int CurrentEnemyCount = 0;
    [SerializeField] private int _enemyCount;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Vector3 _areaSize;

    public List<GameObject> wayPoints;
    private bool spawnBlock;
    private void Start()
    {
        StartCoroutine(EnemySpawn(0));
    }

    private void Update()
    {
        if (CurrentEnemyCount < _enemyCount && !spawnBlock)
        {
            spawnBlock = true;
            StartCoroutine(EnemySpawn(5));
        }
    }

    private IEnumerator EnemySpawn(float timer)
    {
        yield return  new WaitForSeconds(timer);
        for (; CurrentEnemyCount < _enemyCount; CurrentEnemyCount++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, transform);
            enemy.transform.position = NextPosition();
        }

        spawnBlock = false;
    }
    public Vector3 NextPosition() {
        int i = Random.Range(0, wayPoints.Count);
        return transform.TransformPoint(wayPoints[i].transform.position);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _areaSize);
    }
}
