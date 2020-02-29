using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


    public class GuardConnectedPatrol : MonoBehaviour
    {
        public GameObject target;



        public bool patrolWaiting;
        public float totalWaitTime = 3f;
        public float switchProbability = 0.2f;

        NavMeshAgent navMeshAgent;
        ConnectedWaypoint currentWaypoint;
        ConnectedWaypoint previousWaypoint;

        private bool travelling;
        private bool waiting;
        public float waitTimer;
        private int waypointsVisited;

        public void Start()
        {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        
        if (navMeshAgent == null)
            {
                Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
            }
            else
            {   
                if(currentWaypoint == null)
                {
                    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                    if(allWaypoints.Length > 0)
                    {
                        while(currentWaypoint == null)
                        {
                            int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                            ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                            if(startingWaypoint != null)
                            {
                                currentWaypoint = startingWaypoint;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("failed to find any waypoints for use in the scene.");
                    }
                }
            }

           SetDestination();
        }

        public void Update()
        {
        //navMeshAgent.SetDestination(target.transform.position);
        
            if(travelling && navMeshAgent.remainingDistance <= 1.0f)
            {
                travelling = false;
                waypointsVisited++;

                if (patrolWaiting)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    SetDestination();
                }
            }

            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if(waitTimer >= totalWaitTime)
                {
                    waiting = false;

                    SetDestination();
                }
            }
        }


        private void SetDestination()
        {
            if(waypointsVisited > 0)
            {
                ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
                previousWaypoint = currentWaypoint;
                currentWaypoint = nextWaypoint;
            }

            Vector3 targetVector = currentWaypoint.transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }
