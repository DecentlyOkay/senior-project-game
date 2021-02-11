using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public bool waitForClear = false;
    public bool repeat = false;
    [SerializeField] private Wave[] waves = new Wave[0];
    private List<Enemy> aliveEnemies = new List<Enemy>();
    private Bounds spawnArea;

    void Start()
    {
        Debug.Log("Spawner initializing");
        spawnArea = this.GetComponent<Collider>().bounds;
        StartCoroutine(RunSpawner());
    }


    private IEnumerator RunSpawner()
    {
        Debug.Log("Spawner running");
        do 
        {
            foreach (Wave wave in waves)
            {
                yield return SpawnWave(wave);
                Debug.Log("wave done");
                if (waitForClear)
                {
                    yield return new WaitWhile(EnemiesAlive);
                }
            }
        } while (repeat);
        
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        yield return new WaitForSeconds(wave.countDown);
        int index = 0;
        foreach (Enemy enemy in wave.enemies)
        {
            Debug.Log(index);
            
            SpawnEnemy(enemy);
            if(index != wave.enemies.Length - 1)
            {
                Debug.Log("Waiting for " + wave.spawnDelay + " seconds");
                yield return new WaitForSeconds(wave.spawnDelay);
            }
            index++;
        }
    }
    public bool EnemiesAlive()
    {
        // uses Linq to filter out null (previously destroyed) entries and keep only alive enemies
        aliveEnemies = aliveEnemies.Where(e => e != null && e.health > 0).ToList();
        return aliveEnemies.Count > 0;
    }
    private void SpawnEnemy(Enemy enemy)
    {
        Debug.Log("Spawning an enemy");
        Enemy currEnemy = Instantiate(enemy, GetRandomSpawnPosition(), Quaternion.identity);
        foreach(Component c in currEnemy.gameObject.GetComponents<Component>()){
            Debug.Log(c);
        }
        aliveEnemies.Add(currEnemy);
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnArea.min.x, spawnArea.max.x);
        float z = Random.Range(spawnArea.min.z, spawnArea.max.z);
        float y = Random.Range(spawnArea.min.y, spawnArea.max.y);
        return new Vector3(x, y, z);
    }

    [System.Serializable]
    private class Wave
    {
        public Enemy[] enemies = new Enemy[0];
        public float countDown = 0f;
        public float spawnDelay = 0f;
    }
}
