using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        spawning,
        waiting,
        counting,
    }

    [System.Serializable]
    public class Wave
    {
        public Transform enemyPrefab;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public int nextWave = 0;
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 10.5f;
    private float waveCountdown = 3f;

    private float searchCountdown = 1f;

    private SpawnState state;

    public Text currentWaveTextFromWaveSpawner;

    private ObjectPooler objectPooler;
    private UIManager uiManager;

    private void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referred");
        }

        waveCountdown = timeBetweenWaves;

        state = SpawnState.counting;

        objectPooler = ObjectPooler.instance;
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        //objectPooler.SpawnFromPool("Dracula", transform.position, Quaternion.identity);
        if (!uiManager._isGameStarted)
        {
            return;
        }

        int currentWave = nextWave + 1;

        currentWaveTextFromWaveSpawner.text = "Wave: " + currentWave;

        if (state == SpawnState.waiting)
        {
            //check if enemies still alive
            if (!EnemyIsAlive())
            {
                //Begin new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0f)
        {
            if (state != SpawnState.spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private void WaveCompleted()
    {
        state = SpawnState.counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves Completed! Looping....");
        }
        else
        {
            nextWave++;
            
        }
    }

    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
        {
                return false;
            }
        }

        return true;
    }

    private IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.spawning;

        FindObjectOfType<AudioManager>().PlayOneShot("VampireSpawn");

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemyPrefab);

            _wave.rate = Random.Range(1, 3);

            yield return new WaitForSeconds(1/_wave.rate);
        }

        state = SpawnState.waiting;

        yield break;
    }

    private void SpawnEnemy(Transform _enemy)
    {
        Transform sp = spawnPoints[Random.Range(0,spawnPoints.Length)];
        objectPooler.SpawnFromPool("Dracula", sp.position, sp.rotation);
        //Instantiate(_enemy, sp.position, sp.rotation);
    }
}
