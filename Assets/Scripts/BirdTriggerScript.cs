using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTriggerScript : MonoBehaviour
{

    private BirdGuardController birdGuardController;

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(birdGuardController == null)
        {
            GameObject birdGuardControllerObject = GameObject.FindWithTag("Bird");
            birdGuardController = birdGuardControllerObject.GetComponent<BirdGuardController>();
        }
        if(other.tag == "Player")
        {
            birdGuardController.Patrol();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            birdGuardController.Attack();
        }
    }
}
