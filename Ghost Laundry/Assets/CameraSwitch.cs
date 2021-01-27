using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public GameObject ghost;

    private Vector2 mainRoom;

    private Vector2 waitingRoom;

    private Vector2 shop;

    public float lerpTime;

    private bool Inshop; 

    private bool Inwaitingroom; 




    // Start is called before the first frame update
    void Start()
    {
        mainRoom = new Vector2 (0.8266621f,0);
        shop = new Vector2 (-17.58f,0);
        waitingRoom = new Vector2 (0.8266621f,-10.34f);

        Inshop = false; 
        Inwaitingroom = false; 
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D ghostColl){

        ghostColl = ghost.GetComponent<Collider2D>();

        if(ghostColl.gameObject.tag == "PeekPoint" && Inshop == false){

        transform.position = Vector2.Lerp(mainRoom, shop,lerpTime);
        Inshop = true; 
 
        }

         if(ghostColl.gameObject.tag == "PeekPoint" && Inshop == true){

        transform.position = Vector2.Lerp(shop, mainRoom,lerpTime);
        Inshop = false; 
 
        }


        if(ghostColl.gameObject.tag == "PeekPoint2" && Inwaitingroom == false){

        transform.position = Vector2.Lerp(mainRoom, waitingRoom,lerpTime);
        Inwaitingroom = true; 
 
        }

        if(ghostColl.gameObject.tag == "PeekPoint2" && Inwaitingroom == true){

        transform.position = Vector2.Lerp(waitingRoom, mainRoom,lerpTime);
        Inwaitingroom = false; 
 
        }
    }

  
}
