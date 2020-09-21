using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] int score = 0;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    // Update is called once per frame
    void Update()
    {

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

    public IEnumerator FinishLevel(float levelTimer)
    {
        FindObjectOfType<GameUI>().SetLevelTimer(levelTimer);
        StopCoroutine(FindObjectOfType<MeteorSpawner>().spawnRoutine);
        yield return StartCoroutine(FindObjectOfType<GameUI>().FinishLevelAndCountdown(3));
        FindObjectOfType<LevelController>().LoadNextScene();
    }
}
