﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardSimplePatrol : MonoBehaviour
{
    public bool patrolWaiting;
    public float totalWaitTime = 3f;
    public float switchProbability = 0.2f;
    public List<Waypoint> patrolPoints;
    NavMeshAgent navMeshAgent;
    private int currentPatrolIndex;
    private bool travelling;
    private bool waiting;
    private bool patrolForward;
    public float waitTimer;

    // Start is called before the first frame update
    public void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent is not attached to " + gameObject.name);
        }
        else
        {
            if(patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("Insufficient patrol points for basic patrolling behavior.");
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            travelling = false;

            if (patrolWaiting)
            {
                waiting = true;
                waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer >= totalWaitTime)
            {
                waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if(patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }

    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f, 1f) <= switchProbability)
        {
            patrolForward = !patrolForward;
        }

        if (patrolForward)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else
        {
            if(--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }
}
