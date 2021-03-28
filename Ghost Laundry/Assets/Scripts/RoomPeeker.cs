using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPeeker : MonoBehaviour
{

    public CameraSwitch cameraSwitch;
    public Transform room;

    void OnTriggerStay2D(Collider2D ghostColl){

        if(ghostColl.gameObject.tag == "Player") {

            cameraSwitch.SetOffset(transform.position - room.position); 
        }

    }

     void OnTriggerExit2D(Collider2D ghostColl){
        if(ghostColl.gameObject.tag == "Player") {

            cameraSwitch.ResetOffset(); 
        }
    }



}
