using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigs;
    [SerializeField] int startWave = 0;
    [SerializeField] bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int i = startWave; i < waveConfigs.Count; i++)
        {
            yield return StartCoroutine(SpawnWave(waveConfigs[i]));
        }
    }

    private IEnumerator SpawnWave(WaveConfiguration waveToSpawn)
    {
        for (int i = 0; i < waveToSpawn.GetAmountOfEnemies(); i++)
        {
            GameObject enemy = Instantiate(
                waveToSpawn.GetEnemyPrefab(),
                waveToSpawn.GetPathWaypoints()[0].transform.position,
                Quaternion.identity);

            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveToSpawn);
            yield return new WaitForSeconds(waveToSpawn.GetSpawnTime());
        }
    }
}
