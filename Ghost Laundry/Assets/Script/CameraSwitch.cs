using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public GameObject ghost;
    public float lerpTime;

    public Transform mainRoom;
    public Transform shopRoom;
    public Transform waitingRoom;

    private Vector2 destination;
    private Vector2 lastDestination;
    private float timer;

    private Vector2 offset;
    private Vector2 lastOffset;
    private float offsetTimer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        destination = transform.position;
        lastDestination = transform.position;

        offsetTimer = 0.0f;
        offset = Vector2.zero;
        lastOffset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        offsetTimer = Mathf.Clamp(offsetTimer + Time.deltaTime, 0.0f, lerpTime);
        Vector2 currentOffset = Vector2.Lerp(lastOffset, offset, offsetTimer / lerpTime);

        Vector2 offsetDestination = destination + currentOffset;

        timer = Mathf.Clamp(timer + Time.deltaTime, 0.0f, lerpTime);
        transform.position = Vector2.Lerp(lastDestination, offsetDestination, timer / lerpTime);
    }

    public void SetDestination(Vector2 position) {
        lastDestination = transform.position;
        destination = position;
        timer = 0.0f;
    }

    public void SetOffset(Vector2 vector) {
        lastOffset = offset;
        offset = vector;
        offsetTimer = 0.0f;
    }

    public void ResetOffset() {
        lastOffset = offset;
        offset = Vector2.zero;
        offsetTimer = 0.0f;
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
