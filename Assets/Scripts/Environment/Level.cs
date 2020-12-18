using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //player
    GameObject player;

    //level instantiation
    GameObject prevLevel;
    [SerializeField]
    GameObject[] nextLevel;
    [SerializeField]
    GameObject nextLevelSpawnPosition;
    bool levelCompleted;
    //bool allEnemiesSlain = false;

    //spawning propeties
    bool StartSpawn = false;
    [SerializeField]
    GameObject spawnObject;
    [SerializeField]
    int spawnObjectCount = 10;
    [SerializeField]
    float spawnTimer = 1;
    float _curSpawnTimer;
    [SerializeField]
    GameObject[] spawnPosition;
    List<GameObject> enemiesOnLevel = new List<GameObject>();


    void Update()
    {
       Spawn();
        levelCompleteChecker();
       
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Spawn()
    {
        Vector3 _playerPos = player.transform.position;
        Vector3 _spawnPos = transform.position;

        if (levelCompleted == false)
        {
            if (Vector3.Distance(_playerPos, _spawnPos) < 5)
            {
                int objectsToSpawn = spawnObjectCount;
                if (objectsToSpawn > 0)
                {
                    if (_curSpawnTimer <= 0)
                    {
                        int i = spawnPosition.Length;
                        int curSpawnPoint = Random.Range(0, i);


                        GameObject newEnemieOnLevel = Instantiate(spawnObject, spawnPosition[curSpawnPoint].transform.position, spawnPosition[curSpawnPoint].transform.rotation);
                        enemiesOnLevel.Add(newEnemieOnLevel);
                        _curSpawnTimer = spawnTimer;
                        objectsToSpawn -= 1;

                    }
                    else
                    {
                        _curSpawnTimer -= Time.deltaTime;
                    }
                }
            }       
        }        
    }
    void levelCompleteChecker()
    {
        int e = 0;
        foreach (GameObject enemy in enemiesOnLevel)
        {
            if (enemy == null)
            {
                e++;
            }
        }

        if (spawnObjectCount == e)            
        {
            if (levelCompleted == false)
            {
                levelCompleted = true;
                Debug.Log("wewon");
                int index = Random.Range(0, nextLevel.Length);
                Instantiate(nextLevel[index], nextLevelSpawnPosition.transform.position, nextLevelSpawnPosition.transform.rotation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("DOOM SLAYER ENTERS FACILITY!!!");

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Destroy(this.gameObject, 1);

        }
    }

}
