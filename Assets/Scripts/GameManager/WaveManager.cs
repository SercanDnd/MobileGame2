using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> enemySpawnTransforms;
    [SerializeField] private float spawnRate;
    public List<int> waveEnemyCounts;
    public int currentDifficulty;
    public int spawnedEnemyCount;
    private Coroutine _spawnCoroutine;
    public List<EnemyList> EnemyLists;
    private void Start()
    {
        currentDifficulty = 0;
        StartCoroutine(SpawnWave(waveEnemyCounts[currentDifficulty]));
    }


    private IEnumerator SpawnWave(int difficulty)
    {
        for (var i = 0; i <difficulty ; i++)
        {
            var spawnedEnemy = Instantiate(enemyPrefab, EnemySpawnTransform(), quaternion.identity);
            spawnedEnemyCount++;
            yield return new WaitForSeconds(spawnRate);
        }
        
        //Check wave done
        if (difficulty == waveEnemyCounts[^1])
        {
            Debug.Log("Wave bitti");
            //wave done
        }
        else
        {
            NextWave();
        }
    }
    
    private void NextWave()
    {
        spawnedEnemyCount = 0;
        currentDifficulty++;
        Debug.Log("Turn bitti");
        StartCoroutine(SpawnWave(waveEnemyCounts[currentDifficulty]));
    }
    private Vector3 EnemySpawnTransform()
    {
        var randomSpawnTransform = (int)UnityEngine.Random.Range(0, enemySpawnTransforms.Count);
        return enemySpawnTransforms[randomSpawnTransform].transform.position;
    }
}

[System.Serializable]
public class EnemyList
{
    public int level;
    public List<Enemies> EnemiesList;
    
}

[System.Serializable]
public class Enemies
{
    
}


