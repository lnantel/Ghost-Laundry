using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleMovementScript : MonoBehaviour
{
    public float moveSpeed; 

    public CameraShake cameraShake; 
  
    void Start()
    {
        
    }

    void Update()
    {
        //je déclare mes axes 
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        transform.position += new Vector3(X,Y,0) * Time.deltaTime * moveSpeed; 

           if(Input.GetKeyDown(KeyCode.Space)){

            //StartCoroutine(cameraShake.Shake(0.5f,0.1f)); 
        }
    }


}
