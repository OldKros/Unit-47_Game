using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] List<GameObject> meteorPrefabs;
    [SerializeField] List<GameObject> powerUpsToSpawn;

    [Header("Spawning")]
    [SerializeField] float leftPadding = 0.05f;
    [SerializeField] float rightPadding = 0.05f;
    [SerializeField] float minSpawnTimer = 5.0f;
    [SerializeField] float maxSpawnTimer = 10.0f;

    [Header("Travel")]
    [SerializeField] float yMinSpeed = 2f;
    [SerializeField] float yMaxSpeed = 6f;

    float spawnTimer, xMin, xMax;
    public Coroutine spawnRoutine { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        SetSpawnBoundaries();
        spawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
        spawnRoutine = StartCoroutine(SpawnMeteors());
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
    }

    void SetSpawnBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x + leftPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - rightPadding;
    }

    IEnumerator SpawnMeteors()
    {
        Vector2 spawnLoc, target;
        while (true)
        {
            yield return new WaitUntil(() => spawnTimer <= 0);
            float yVelocity = Random.Range(-yMinSpeed, -yMaxSpeed);
            spawnLoc = RandomSpawnLoc();
            target = GetTarget();
            // xVel = the distance along the x axis divided by
            // the distance along the y axis divided by the speed it travel the y axis
            float xVelocity = (spawnLoc.x - target.x) / ((spawnLoc.y - target.y) / yVelocity);

            int meteorToSpawn = Random.Range(0, meteorPrefabs.Count - 1);
            var meteor = Instantiate(meteorPrefabs[meteorToSpawn],
                                    spawnLoc, Quaternion.identity) as GameObject;
            meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
            meteor.GetComponent<Meteor>().SetSpinSpeed(Random.Range(-3, -1));
            meteor.GetComponent<Meteor>().SetPowerUps(powerUpsToSpawn);

            spawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
        }
    }

    Vector2 RandomSpawnLoc()
    {
        float x = Random.Range(xMin, xMax);
        float y = transform.position.y;

        return new Vector2(x, y);
    }

    Vector2 GetTarget()
    {
        float x = Random.Range(xMin, xMax);
        float y = -transform.position.y;

        return new Vector2(x, y);
    }
}
