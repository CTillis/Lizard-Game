using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : MonoBehaviour
{
    [SerializeField]
    Transform destination;

    NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();


        if(navMeshAgent == null)
        {
            Debug.LogError("The navmesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
    }

    
    private void SetDestination()
    {
        if(destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }
}