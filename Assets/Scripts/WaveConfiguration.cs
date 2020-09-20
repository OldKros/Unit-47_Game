using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Configuration")]
public class WaveConfiguration : ScriptableObject
{
    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int amountOfEnemies = 5;
    [SerializeField] float enemyMoveSpeed = 2.0f;

    [Header("Path")]
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnTimeRandomFactor = 0.3f;
    [SerializeField] float loopDelay = 2.0f;
    [SerializeField] int loopAmount = 1;
    [SerializeField] float initialDelay = 0.0f;
    [SerializeField] bool needAllDestroyed = false;
    [SerializeField] bool isDefeated = false;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public float GetEnemyMoveSpeed() { return enemyMoveSpeed; }

    public int GetAmountOfEnemies() { return amountOfEnemies; }

    public List<Transform> GetPathWaypoints()
    {
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform wp in pathPrefab.transform)
        {
            waypoints.Add(wp);
        }
        return waypoints;
    }

    public float GetLoopDelay() { return loopDelay; }

    public int GetLoopCount() { return loopAmount; }

    public float RandomSpawnTime()
    {
        return timeBetweenSpawns + Random.Range(0.0f, spawnTimeRandomFactor);
    }

    public float GetInitialDelay() { return initialDelay; }

    public bool NeedAllDestroyed() { return needAllDestroyed; }

    public bool IsDefeated() { return isDefeated; }

    public void SetIsDefeated(bool isDefeated) { this.isDefeated = isDefeated; }

}
