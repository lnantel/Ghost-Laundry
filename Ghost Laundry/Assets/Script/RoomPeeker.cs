using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPeeker : MonoBehaviour
{

    public CameraSwitch cameraSwitch;
    public GameObject ghost;
    public Transform room;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D ghostColl){

        if(ghostColl.gameObject.tag == "Player"){

            cameraSwitch.SetOffset(transform.position - room.position); 
        }

    }

     void OnTriggerExit2D(Collider2D ghostColl){

        if(ghostColl.gameObject.tag == "Player") {

            cameraSwitch.ResetOffset(); 
        }
    }



}
