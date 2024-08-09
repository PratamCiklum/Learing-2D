using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    private float yPos = 4;
    private float yNeg = 1;
    private float xPos = 7.75f;
    private ObjectPooler objectPooler;
    private float spawnRate = 2;
    private float delay = 3;
    private string[] enemyName = { "Shooter", "Attacker" };
    // Start is called before the first frame update
    void Start()

    {
        objectPooler = ObjectPooler.Instance;
        InvokeRepeating("Spawn", delay, spawnRate);
    }

    private void Spawn()
    {
        int index = Random.Range(0, enemyName.Length);
        float yRange = Random.Range(yNeg, yPos);
        float xRange = Random.Range(-xPos, xPos);
        objectPooler.spawnFromPool(enemyName[index], new Vector2(xRange, yRange), Quaternion.identity, Vector2.down);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
