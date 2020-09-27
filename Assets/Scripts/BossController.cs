using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] WaveConfiguration bossWave;

    GameState gameState;
    GameObject boss;
    bool started = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        SpawnBoss();
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBossCompletion();
    }

    void SpawnBoss()
    {
        if (FindObjectOfType<Boss>() != null)
            boss = FindObjectOfType<Boss>().gameObject;
        else
            boss = Instantiate(bossWave.GetEnemyPrefab(),
                                bossWave.GetPathWaypoints()[0].transform.position,
                                Quaternion.identity);
        boss.GetComponent<EnemyPathing>().SetWaveConfig(bossWave);
        boss.GetComponent<EnemyPathing>().SetAlwaysLoop(true);
    }

    void CheckBossCompletion()
    {
        if (boss == null)
        {
            gameState.bossControllerFinished = true;
        }
    }
}
