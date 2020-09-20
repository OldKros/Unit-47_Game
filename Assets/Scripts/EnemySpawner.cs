using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigs;
    [SerializeField] int startWave = 0;
    [SerializeField] float delayBetweenWaves = 2.0f;
    [SerializeField] List<EnemyWave> enemyWaves;
    bool looping = true;
    float loopTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
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

    private IEnumerator InitialWaveSpawning(WaveConfiguration waveConfig)
    {
        // Debug.Log("Commencing Spawning");
        var enemyWave = new EnemyWave();
        enemyWaves.Add(enemyWave);
        yield return new WaitForSeconds(waveConfig.GetInitialDelay());
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
        if (waveConfig.NeedAllDestroyed())
            StartCoroutine(LoopWaveUntilDead(waveConfig, enemyWave));
        else
            StartCoroutine(LoopWave(waveConfig, enemyWave));
    }

    private IEnumerator LoopWave(WaveConfiguration waveConfig, EnemyWave enemyWave)
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

    private IEnumerator LoopWaveUntilDead(WaveConfiguration waveConfig, EnemyWave enemyWave)
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

    private IEnumerator RestartWave(WaveConfiguration waveConfig, EnemyWave enemyWave)
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

    private bool AreAllWavesFinishedOrDestroyed()
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
            yield return new WaitForSeconds(2);
        }
        Debug.Log("Level Complete");
        yield return StartCoroutine(FindObjectOfType<GameUI>().Countdown(3));
        FindObjectOfType<LevelController>().LoadNextScene();
    }

    [System.Serializable]
    public class EnemyWave
    {
        public List<GameObject> enemyList { get; private set; }

        public EnemyWave()
        {
            enemyList = new List<GameObject>();
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
