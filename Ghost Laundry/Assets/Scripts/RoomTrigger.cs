using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    public CameraSwitch cameraSwitch;
    public GameObject ghost; 

    void OnTriggerEnter2D(Collider2D ghostColl){

        if(ghostColl.gameObject.tag == "Player"){
            cameraSwitch.SetDestination(transform.position);
        }
    }

}
