using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class BirdGuardController : MonoBehaviour
{
    public GameObject target;

    public bool patrolWaiting;
    public float totalWaitTime = 3f;
    public float switchProbability = 0.2f;
    public bool isAttacking;

    NavMeshAgent navMeshAgent;
    ConnectedWaypoint currentWaypoint;
    ConnectedWaypoint previousWaypoint;

    private bool travelling;
    private bool waiting;
    public float waitTimer;
    private int waypointsVisited;
    private Animator anim;
    AudioSource audioControl;
    public AudioSource audioAttack;

    public void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        Attack();
        audioControl = GetComponent<AudioSource>();
        audioControl.Play();
    }
    
    public void FixedUpdate()
    {
        //anim.SetInteger("state", 1) means regular walk animation, ("state", 2) means sped up animation. ("state", 0) means he won't animate aka idle
        if (isAttacking == true)
        {
            anim.SetInteger("state", 2);
            navMeshAgent.SetDestination(target.transform.position);
            GameObject.Find("UIManager").GetComponent<UIManager>().spotted = true;
        }
        else
            GameObject.Find("UIManager").GetComponent<UIManager>().spotted = false;

        if (travelling && navMeshAgent.remainingDistance <= 1.0f)
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
            if (waitTimer >= totalWaitTime)
            {
                waiting = false;

                SetDestination();
            }
        }

        
    }

    public void Update()
    {
        if (Time.timeScale == 0)
            audioControl.Stop();

        if (Time.timeScale == 1 && !audioControl.isPlaying)
            audioControl.Play();
    }

    public void Patrol()
    {
        isAttacking = false;
        anim.SetInteger("state", 1);
        if (navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            if (currentWaypoint == null)
            {
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        if (startingWaypoint != null)
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

    public void SetDestination()
    {
        if (waypointsVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
            previousWaypoint = currentWaypoint;
            currentWaypoint = nextWaypoint;
        }

        Vector3 targetVector = currentWaypoint.transform.position;
        navMeshAgent.SetDestination(targetVector);
        travelling = true;
    }
    
    public void Attack()
    {
        isAttacking = true;
        audioAttack.Play();
    }

}
