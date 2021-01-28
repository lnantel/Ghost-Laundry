using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public GameObject ghost;

   

  

    public float lerpTime;

   




    // Start is called before the first frame update
    void Start()
    {
     
  
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


public void Move(Vector2 destination){

    
    transform.position = Vector2.Lerp(transform.position, destination,lerpTime);
    

}

public void Peek(Vector2 peekdestination){

    
    transform.position = Vector2.Lerp(transform.position, peekdestination,lerpTime);

}


  
}
