using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigs;
    [SerializeField] int startWave = 0;
    [SerializeField] float delayBetweenWaves = 2.0f;
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
            yield return StartCoroutine(CommenceWaveSpawning(waveConfigs[i]));
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    private IEnumerator CommenceWaveSpawning(WaveConfiguration waveConfig)
    {
        Debug.Log("Commencing Spawning");
        var enemyWave = new List<GameObject>();
        yield return new WaitForSeconds(waveConfig.GetInitialDelay());
        for (int i = 0; i < waveConfig.GetAmountOfEnemies(); i++)
        {
            var enemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetPathWaypoints()[0].transform.position,
                Quaternion.identity) as GameObject;
            enemyWave.Add(enemy);
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
        }

        if (waveConfig.NeedAllDestroyed())
            StartCoroutine(LoopWaveUntilDead(waveConfig, enemyWave));
        else
            StartCoroutine(LoopWave(waveConfig, enemyWave));
    }

    private IEnumerator LoopWave(WaveConfiguration waveConfig, List<GameObject> enemyWave)
    {
        Debug.Log("Beginning Looping");
        int loopCtr = waveConfig.GetLoopCount();
        yield return new WaitUntil(() => WaveFinished(waveConfig, enemyWave));
        do
        {
            yield return new WaitForSeconds(waveConfig.GetLoopDelay());
            yield return StartCoroutine(ResetWave(waveConfig, enemyWave));
            yield return new WaitUntil(() => WaveFinished(waveConfig, enemyWave));
            loopCtr--;
            if (WaveDestroyed(enemyWave))
                break;
        } while (loopCtr > 0);
        Debug.Log("Succesfully looped");
        DestroyAllEnemiesInWave(enemyWave);
    }

    private IEnumerator LoopWaveUntilDead(WaveConfiguration waveConfig, List<GameObject> enemyWave)
    {
        Debug.Log("Beginning LoopUntilDead");
        while (!WaveDestroyed(enemyWave))
        {
            yield return new WaitUntil(() => WaveFinished(waveConfig, enemyWave));
            yield return StartCoroutine(ResetWave(waveConfig, enemyWave));
            yield return new WaitForSeconds(waveConfig.GetLoopDelay());
        }
    }

    private IEnumerator ResetWave(WaveConfiguration waveConfig, List<GameObject> enemyWave)
    {
        int ctr = 0;
        foreach (var enemy in enemyWave)
        {
            Debug.Log("Moving item #" + ctr);
            if (enemy != null)
                enemy.GetComponent<EnemyPathing>().ResetPosition();
            ctr++;
            yield return new WaitForSeconds(waveConfig.RandomSpawnTime());
        }
    }

    private bool WaveDestroyed(List<GameObject> enemyWave)
    {
        foreach (var enemy in enemyWave)
        {
            if (enemy != null)
                return false;
        }
        Debug.Log("All enemies Dead");
        return true;
    }

    private bool WaveFinished(WaveConfiguration waveConfig, List<GameObject> enemyWave)
    {
        int size = waveConfig.GetPathWaypoints().Count;
        foreach (var enemy in enemyWave)
        {
            if (enemy != null && !enemy.GetComponent<EnemyPathing>().FinishedRoute())
            {
                return false;
            }
        }
        return true;
    }

    private void DestroyAllEnemiesInWave(List<GameObject> enemyWave)
    {
        foreach (var enemy in enemyWave)
        {
            Destroy(enemy.gameObject);
        }
    }
}
