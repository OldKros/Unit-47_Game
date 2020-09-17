﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfiguration waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    bool finishedRoute = false;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetPathWaypoints();
        transform.position = waypoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
    }
    public bool FinishedRoute() { return finishedRoute; }
    public void SetWaveConfig(WaveConfiguration waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void FollowPath()
    {
        if (waypointIndex < waypoints.Count)
        {

            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetEnemyMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
                waypointIndex++;
        }
        else
        {
            finishedRoute = true;
        }
    }

    public void ResetPosition()
    {
        finishedRoute = false;
        waypointIndex = 0;
        transform.position = waypoints[waypointIndex].transform.position;
    }
}
