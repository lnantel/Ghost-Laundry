using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPeeker : MonoBehaviour
{

    public CameraSwitch cameraSwitch;
    public GameObject ghost;

    public Transform mainRoom; 

    public Transform waitingRoom; 

    public Transform shop;  
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

            cameraSwitch.Peek(transform.position); 
        }

    
    }

     void OnTriggerExit2D(Collider2D ghostColl){

        if(ghostColl.gameObject.name == "WaitingRoom"){

            cameraSwitch.Move(waitingRoom.position); 
        }

          if(ghostColl.gameObject.name == "Shop"){

            cameraSwitch.Move(shop.position); 
        }

         if(ghostColl.gameObject.name == "MainRoom"){

            cameraSwitch.Move(mainRoom.position); 
        }

    
    }



}
