using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{

    public CameraSwitch cameraSwitch;
    public GameObject ghost; 
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
        
        StartCoroutine(cameraSwitch.Move(transform.position));
        
        }
}

}
