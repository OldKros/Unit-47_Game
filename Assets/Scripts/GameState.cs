using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] int score = 0;

    EnemySpawner enemySpawner;
    BossController bossController;

    public bool enemySpawnerFinished { get; set; }
    public bool bossControllerFinished { get; set; }
    float levelTimer;

    // Start is called before the first frame update
    void Awake()
    {
        levelTimer = 0f;
        enemySpawner = FindObjectOfType<EnemySpawner>();
        bossController = FindObjectOfType<BossController>();
        enemySpawnerFinished = false;
        bossControllerFinished = false;
        SetUpSingleton();
        StartCoroutine(WaitAndFinishLevel());
    }

    // Update is called once per frame
    void Update()
    {
        levelTimer += Time.deltaTime;
    }

    void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore() { return score; }

    public void AddScore(int score) { this.score += score; }

    IEnumerator WaitAndFinishLevel()
    {
        if (enemySpawner == null)
            enemySpawnerFinished = true;
        if (bossController == null)
            bossControllerFinished = true;

        yield return new WaitUntil(() => enemySpawnerFinished && bossControllerFinished);
        FindObjectOfType<GameUI>().SetLevelTimer(levelTimer);
        score = 0;
        StopCoroutine(FindObjectOfType<MeteorSpawner>().spawnRoutine);
        yield return StartCoroutine(FindObjectOfType<GameUI>().FinishLevelAndCountdown(3));
        FindObjectOfType<LevelController>().LoadNextScene();
    }
}
