using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public int CurrentEnemyCount = 0;
    [SerializeField] private int _enemyCount;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Vector3 _areaSize;

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
    public Vector3 NextPosition()
    {
        float x = Random.Range(-_areaSize.x/2, _areaSize.x/2);
        float z = Random.Range(-_areaSize.z/2, _areaSize.z/2);

        return transform.TransformPoint(x, 0, z);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _areaSize);
    }
}
