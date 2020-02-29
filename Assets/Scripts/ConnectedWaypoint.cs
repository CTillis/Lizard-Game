using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWaypoint : Waypoint
{
    // Adjust this to tell exactly how far away the the waypoint can see another waypoint from.
    protected float connectivityRadius = 20f;

    public List<ConnectedWaypoint> connections;


    public void Start()
    {
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        connections = new List<ConnectedWaypoint>();

        for(int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();
            
            if(nextWaypoint != null)
            {
                if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        //Will let you know that there aren't any waypoints nearby
        if(connections.Count == 0)
        {
            Debug.LogError("Insufficient waypoint count.");
            return null;
        }
        //Will essentially be a dead end
        else if(connections.Count == 1 && connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }
        //The actual scripting that grabs the other waypoints
        else
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);
            
            return nextWaypoint;
            
        }
    }
}
