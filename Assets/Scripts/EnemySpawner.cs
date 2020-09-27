using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigs;
    [SerializeField] int startWave = 0;
    [SerializeField] float delayBetweenWaves = 2.0f;
    List<EnemyWave> enemyWaves;
    float levelTimer = 0.0f;
    GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        StartCoroutine(SpawnAllWaves());
    }

    void Update()
    {
        levelTimer += Time.deltaTime;
    }

    IEnumerator SpawnAllWaves()
    {
        for (int i = startWave; i < waveConfigs.Count; i++)
        {
            Debug.Log("Spawning wave: " + i);
            if (waveConfigs[i].NeedAllDestroyed())
                StartCoroutine(LoopWaveUntilDead(waveConfigs[i]));
            else
                StartCoroutine(LoopWave(waveConfigs[i]));
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        Debug.Log("Finished Spawning Waves");
        StartCoroutine(CheckLevelProgress());
    }

    IEnumerator LoopWave(WaveConfiguration waveConfig)
    {
        var enemyWave = new EnemyWave();
        enemyWaves.Add(enemyWave);
        yield return new WaitUntil(() => levelTimer >= waveConfig.GetInitialDelay());
        int loopCtr = waveConfig.GetLoopCount();

        while (loopCtr > 0)
        {
            yield return new WaitForSeconds(waveConfig.GetLoopDelay());
            for (int i = 0; i < waveConfig.GetAmountOfEnemies(); i++)
            {
                var enemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetPathWaypoints()[0].transform.position,
                    Quaternion.identity);
                enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                enemyWave.AddEnemy(enemy);
                yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
            }
            enemyWave.allSpawned = true;
            yield return new WaitUntil(() => enemyWave.IsWaveFinished());
            enemyWave.allSpawned = false;
            enemyWave.DestroyAllEnemiesInWave();
            loopCtr--;
        }
        enemyWave.waveComplete = true;
    }

    IEnumerator LoopWaveUntilDead(WaveConfiguration waveConfig)
    {
        var enemyWave = new EnemyWave();
        enemyWaves.Add(enemyWave);
        yield return new WaitUntil(() => levelTimer >= waveConfig.GetInitialDelay());
        for (int i = 0; i < waveConfig.GetAmountOfEnemies(); i++)
        {
            var enemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetPathWaypoints()[0].transform.position,
                Quaternion.identity);
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            enemyWave.AddEnemy(enemy);
            yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
        }
        enemyWave.allSpawned = true;

        while (!enemyWave.IsWaveDead())
        {
            yield return new WaitUntil(() => enemyWave.IsWaveFinished());
            Debug.Log("Resetting LoopWaveUntilDead wave");
            yield return StartCoroutine(RestartWave(waveConfig, enemyWave));
            yield return new WaitForSeconds(waveConfig.GetLoopDelay());
        }
        enemyWave.waveComplete = true;
    }

    IEnumerator RestartWave(WaveConfiguration waveConfig, EnemyWave enemyWave)
    {
        enemyWave.allSpawned = false;
        int ctr = 0;
        foreach (var enemy in enemyWave.enemyList)
        {
            // Debug.Log("Moving item #" + ctr);
            if (enemy != null)
                enemy.GetComponent<EnemyPathing>().ResetPosition();
            ctr++;
            yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
        }
        enemyWave.allSpawned = true;
    }

    IEnumerator CheckLevelProgress()
    {
        yield return new WaitUntil(() => AreAllWavesFinishedOrDestroyed());
        Debug.Log("Level Complete");
        gameState.enemySpawnerFinished = true; ;
    }

    bool AreAllWavesFinishedOrDestroyed()
    {
        foreach (var enemyWave in enemyWaves)
        {
            if (!enemyWave.waveComplete)
                return false;
        }
        return true;
    }

    [System.Serializable]
    public class EnemyWave
    {
        public List<GameObject> enemyList { get; private set; }
        public bool allSpawned { get; set; }
        public bool waveComplete { get; set; }

        public EnemyWave()
        {
            enemyList = new List<GameObject>();
            allSpawned = false;
            waveComplete = false;
        }

        public void AddEnemy(GameObject enemy)
        {
            enemyList.Add(enemy);
        }

        public void DestroyAllEnemiesInWave()
        {
            foreach (var enemy in enemyList)
            {
                Destroy(enemy.gameObject);
            }
            enemyList.Clear();
        }

        public bool IsWaveDead()
        {
            if (!allSpawned)
                return false;

            foreach (var enemy in enemyList)
            {
                if (enemy != null)
                    return false;
            }
            Debug.Log("All enemies Dead");
            return true;
        }

        public bool IsWaveFinished()
        {
            if (!allSpawned)
                return false;

            foreach (var enemy in enemyList)
            {
                if (enemy != null && !enemy.GetComponent<EnemyPathing>().FinishedRoute())
                {
                    return false;
                }
            }
            Debug.Log("All enemies finished");
            return true;
        }
    }
}
