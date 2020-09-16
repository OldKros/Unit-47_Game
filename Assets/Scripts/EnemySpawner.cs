using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigs;
    [SerializeField] int startWave = 0;
    [SerializeField] float waveDelay = 2.0f;
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
            StartCoroutine(SpawnWave(waveConfigs[i]));
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private IEnumerator SpawnWave(WaveConfiguration waveToSpawn)
    {
        int loopCtr = waveToSpawn.GetLoopAmount();
        yield return new WaitForSeconds(waveToSpawn.GetInitialDelay());
        do
        {
            for (int i = 0; i < waveToSpawn.GetAmountOfEnemies(); i++)
            {
                var enemy = Instantiate(
                    waveToSpawn.GetEnemyPrefab(),
                    waveToSpawn.GetFirstWaypoint().transform.position,
                    Quaternion.identity);

                enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveToSpawn);
                yield return new WaitForSeconds(waveToSpawn.RandomSpawnTime());

            }
            loopCtr--;
            yield return new WaitForSeconds(waveToSpawn.GetLoopDelay());
        } while (loopCtr >= 0);
    }
}
