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


public IEnumerator Move (Vector2 destination){

    float time = 0f; 

    while (time < lerpTime) {

     transform.position = Vector2.Lerp(transform.position, destination,time / lerpTime);
     time += Time.deltaTime; 
     yield return new WaitForSeconds(0); 

    }


    

}

public void Peek(Vector2 peekdestination){

    
    transform.position = Vector2.Lerp(transform.position, peekdestination,lerpTime);

}




  
}
