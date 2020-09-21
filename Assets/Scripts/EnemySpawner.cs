using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigs;
    [SerializeField] int startWave = 0;
    [SerializeField] float delayBetweenWaves = 2.0f;
    [SerializeField] List<EnemyWave> enemyWaves;
    float levelTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
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
            // Debug.Log("Spawning " + waveConfigs[i]);
            StartCoroutine(InitialWaveSpawning(waveConfigs[i]));
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        Debug.Log("Finished Spawning Waves");
        StartCoroutine(CheckLevelProgress());
    }

    IEnumerator InitialWaveSpawning(WaveConfiguration waveConfig)
    {
        // Debug.Log("Commencing Spawning");
        var enemyWave = new EnemyWave();
        enemyWaves.Add(enemyWave);
        yield return new WaitUntil(() => levelTimer >= waveConfig.GetInitialDelay());
        for (int i = 0; i < waveConfig.GetAmountOfEnemies(); i++)
        {
            Debug.Log("Spawning enemy " + i);
            var enemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetPathWaypoints()[0].transform.position,
                Quaternion.identity) as GameObject;

            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            enemyWave.AddEnemy(enemy);
            yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
        }
        enemyWave.SetFinishedSpawning(true);
        if (waveConfig.NeedAllDestroyed())
            StartCoroutine(LoopWaveUntilDead(waveConfig, enemyWave));
        else
            StartCoroutine(LoopWave(waveConfig, enemyWave));
    }

    IEnumerator LoopWave(WaveConfiguration waveConfig, EnemyWave enemyWave)
    {
        // Debug.Log("Beginning Looping");
        int loopCtr = waveConfig.GetLoopCount();

        // wait until the wave has finished before looping
        yield return StartCoroutine(enemyWave.WaitUntilFinished());

        while (loopCtr > 0)
        {
            yield return new WaitForSeconds(waveConfig.GetLoopDelay());
            yield return StartCoroutine(RestartWave(waveConfig, enemyWave));
            yield return StartCoroutine(enemyWave.WaitUntilFinished());
            loopCtr--;
            if (enemyWave.IsWaveDead())
            {
                break;
            }
        }
        // Debug.Log("Succesfully looped");
        if (enemyWave.IsWaveDead())
            enemyWave.DestroyAllEnemiesInWave();
    }

    IEnumerator LoopWaveUntilDead(WaveConfiguration waveConfig, EnemyWave enemyWave)
    {
        bool finished = false;

        while (!enemyWave.IsWaveDead())
        {
            while (!finished)
            {
                finished = enemyWave.IsWaveFinished();
                yield return new WaitForSeconds(0.5f);
            }
            yield return StartCoroutine(RestartWave(waveConfig, enemyWave));
            yield return new WaitForSeconds(waveConfig.GetLoopDelay());
        }
    }

    IEnumerator RestartWave(WaveConfiguration waveConfig, EnemyWave enemyWave)
    {
        int ctr = 0;
        foreach (var enemy in enemyWave.enemyList)
        {
            // Debug.Log("Moving item #" + ctr);
            if (enemy != null)
                enemy.GetComponent<EnemyPathing>().ResetPosition();
            ctr++;
            yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
        }
    }

    bool AreAllWavesFinishedOrDestroyed()
    {
        foreach (var enemyWave in enemyWaves)
        {
            if (!enemyWave.IsWaveDead() || !enemyWave.IsWaveFinished())
                return false;
        }
        return true;
    }

    IEnumerator CheckLevelProgress()
    {
        bool completed = false;
        while (!completed)
        {
            completed = AreAllWavesFinishedOrDestroyed();
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Level Complete");
        StartCoroutine(FindObjectOfType<GameState>().FinishLevel(levelTimer));
    }

    [System.Serializable]
    public class EnemyWave
    {
        public List<GameObject> enemyList { get; private set; }

        public bool finishedSpawning { get; private set; }

        public EnemyWave()
        {
            enemyList = new List<GameObject>();
            finishedSpawning = false;
        }

        public void SetFinishedSpawning(bool b)
        {
            finishedSpawning = b;
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
        }

        public bool IsWaveDead()
        {
            if (!finishedSpawning)
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
            if (!finishedSpawning)
                return false;

            foreach (var enemy in enemyList)
            {
                if (enemy != null && !enemy.GetComponent<EnemyPathing>().FinishedRoute())
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerator WaitUntilFinished()
        {
            bool finished = false;
            while (!finished)
            {
                finished = IsWaveFinished();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
