using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfiguration : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float spawnTime = 0.5f;
    [SerializeField] float spawnTimeRandomFactor = 0.3f;
    [SerializeField] int amountOfEnemies = 5;
    [SerializeField] float enemyMoveSpeed = 2f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public List<Transform> GetPathWaypoints()
    {
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform wp in pathPrefab.transform)
        {
            waypoints.Add(wp);
        }
        return waypoints;
    }

    public float GetSpawnTime() { return spawnTime; }

    public float GetSpawnTimeRandomFactor() { return spawnTimeRandomFactor; }

    public int GetAmountOfEnemies() { return amountOfEnemies; }

    public float GetEnemyMoveSpeed() { return enemyMoveSpeed; }
}
