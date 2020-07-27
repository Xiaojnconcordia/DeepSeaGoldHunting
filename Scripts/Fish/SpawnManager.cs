using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject shark;
    public GameObject octopus;
    public GameObject[] goldBars;

    private float spawnRangeX = 93.0f;
    private float spawnRangeY = 98.0f;
    private float spawnRangeZ = 93.0f;
    private float spawnGoldY = -99.0f;

    private float startDelay = 2.0f;
    [SerializeField]
    private float spawnInterval = 20.0f;

    [SerializeField]
    private float lifeTimeEnemy = 40f;
    private float lifeTimeGold = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnShark", startDelay, spawnInterval);

        // We know in the MoveOctopus.cs, a level lasts 40 seconds
        // So, octopus will spawn at least twice in a level in random time. 
        // And we don't want it spawn too fast, so make the left range bigger
        InvokeRepeating("SpawnOctopus", Random.Range(5, 11), Random.Range(10, 31));
        InvokeRepeating("SpawnGoldBar", startDelay, spawnInterval);
    }

    private void SpawnShark()
    {
        float randomX = Random.Range(spawnRangeX, -spawnRangeX);
        float randomY = Random.Range(spawnRangeY, -spawnRangeY);
        float randomZ = Random.Range(spawnRangeZ, -spawnRangeZ);
        float randomSize = Random.Range(40.0f, 80.0f);
        Vector3 spawnPos = new Vector3(randomX, randomY, randomZ);

        GameObject enemy = Instantiate(shark, spawnPos, shark.transform.rotation);
        //size varies when spawn
        enemy.transform.localScale = Vector3.one * randomSize;
        Destroy(enemy, lifeTimeEnemy);
    }

    private void SpawnOctopus()
    {
        float randomX = Random.Range(spawnRangeX, -spawnRangeX);
        float randomY = Random.Range(spawnRangeY, -spawnRangeY);
        float randomZ = Random.Range(spawnRangeZ, -spawnRangeZ);
        Vector3 spawnPos = new Vector3(randomX, randomY, randomZ);
        GameObject enemy = Instantiate(octopus, spawnPos, octopus.transform.rotation);
        Destroy(enemy, lifeTimeEnemy);
    }

    private void SpawnGoldBar()
    {
        int randomIndex = Random.Range(0, goldBars.Length);
        float randomX = Random.Range(spawnRangeX, -spawnRangeX);
        float randomZ = Random.Range(spawnRangeZ, -spawnRangeZ);
        Vector3 spawnPos = new Vector3(randomX, spawnGoldY, randomZ); //only spawn on the see floor

        //spawn random gold bar (1 point, 2points, 10 points)
        GameObject gold = Instantiate(goldBars[randomIndex], spawnPos, goldBars[randomIndex].transform.rotation);
        Destroy(gold, lifeTimeGold);
    }
}
