using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] List<GameObject> meteorPrefabs;

    [Header("Spawning")]
    [SerializeField] float minSpawnTimer = 5.0f;
    [SerializeField] float maxSpawnTimer = 10.0f;

    [Header("Travel")]
    [SerializeField] float minX = -3.2f;
    [SerializeField] float maxX = 3.2f;
    [SerializeField] float minSpeed = -2f;
    [SerializeField] float maxSpeed = -6f;
    float meteorSpeedDown = -6f;

    float spawnTimer;
    public Coroutine spawnRoutine { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
        spawnRoutine = StartCoroutine(SpawnMeteors());
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
    }

    IEnumerator SpawnMeteors()
    {
        while (true)
        {
            yield return new WaitUntil(() => spawnTimer <= 0);
            meteorSpeedDown = Random.Range(minSpeed, maxSpeed);
            Vector2 spawnLoc = RandomSpawnLoc();
            Vector2 target = GetTarget();
            float xVelocity = (spawnLoc.x - target.x) / ((spawnLoc.y - target.y) / meteorSpeedDown);

            int meteorToSpawn = Random.Range(0, meteorPrefabs.Count - 1);
            var meteor = Instantiate(meteorPrefabs[meteorToSpawn],
                                    spawnLoc, Quaternion.identity) as GameObject;
            meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, meteorSpeedDown);

            spawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
        }
    }

    Vector2 RandomSpawnLoc()
    {
        float x = Random.Range(minX, maxX);
        float y = gameObject.transform.position.y;

        return new Vector2(x, y);
    }


    Vector2 GetTarget()
    {
        float x = Random.Range(minX, maxX);
        float y = -transform.position.y;

        return new Vector2(x, y);
    }
}
