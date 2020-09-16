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
    List<Transform> waypoints = new List<Transform>();
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnTimeRandomFactor = 0.3f;
    [SerializeField] float loopDelay = 2.0f;
    [SerializeField] int loopAmount = 1;
    [SerializeField] float initialDelay = 0.0f;


    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public float GetEnemyMoveSpeed() { return enemyMoveSpeed; }

    public int GetAmountOfEnemies() { return amountOfEnemies; }

    public Transform GetFirstWaypoint()
    {
        if (waypoints.Count == 0)
            SetWaypoints();
        return waypoints[0];
    }

    public List<Transform> GetPathWaypoints()
    {
        if (waypoints.Count == 0)
            SetWaypoints();
        return waypoints;
    }

    private void SetWaypoints()
    {
        foreach (Transform wp in pathPrefab.transform)
        {
            waypoints.Add(wp);
        }
    }

    public float GetLoopDelay() { return loopDelay; }

    public int GetLoopAmount() { return loopAmount; }

    public float RandomSpawnTime()
    {
        return timeBetweenSpawns + Random.Range(0.0f, spawnTimeRandomFactor);
    }

    public float GetInitialDelay() { return initialDelay; }
}
