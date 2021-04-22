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

    public float zPos;

    private int lastRoomIndex;
    private Vector2 destination;
    private Vector2 lastDestination;
    private float timer;

    private Vector2 offset;
    private Vector2 lastOffset;
    private Vector2 currentOffset;
    private float offsetTimer;

    private Vector3 lastPos;
    private Vector3 velocity;

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
    void LateUpdate()
    {
        if (lastRoomIndex != PlayerStateManager.instance.CurrentRoomIndex) {
            Vector2 currentRoomPos = PlayerStateManager.instance.Rooms[PlayerStateManager.instance.CurrentRoomIndex].transform.position;
            lastDestination = transform.position;
            destination = currentRoomPos;
            timer = 0.0f;
            lastRoomIndex = PlayerStateManager.instance.CurrentRoomIndex;
        }

        if (PlayerStateManager.instance.CurrentRoomIndex == 1) {
            destination = new Vector2(destination.x, Mathf.Max(PlayerStateManager.instance.transform.position.y, -10.0f));
        }

        offsetTimer = Mathf.Clamp(offsetTimer + Time.deltaTime, 0.0f, lerpTime);

        float t = offsetTimer / lerpTime;
        t = t * t * (3f - 2f * t);
        currentOffset = Vector2.Lerp(lastOffset, offset, t);

        Vector2 offsetDestination = destination + currentOffset;

        timer = Mathf.Clamp(timer + Time.deltaTime, 0.0f, lerpTime);
        t = timer / lerpTime;
        t = t * t * (3f - 2f * t);
        if(timer >= lerpTime) {
            Vector3 offsetDestination3 = new Vector3(offsetDestination.x, offsetDestination.y, zPos);
            //transform.position = Vector3.SmoothDamp(transform.position, offsetDestination3, ref velocity, 0.1f);
            transform.position = offsetDestination3;
        }
        else {
            transform.position = Vector2.Lerp(lastDestination, offsetDestination, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
            velocity = transform.position - lastPos;
        }
        lastPos = transform.position;
    }

    public void SetDestination(Vector2 position) {
        //lastDestination = transform.position;
        //destination = position;
        //timer = 0.0f;
    }

    public void SetOffset(Vector2 vector) {
        if (!vector.Equals(offset)) {
            lastOffset = offset;
            offset = vector;
            offsetTimer = 0.0f;
        }
    }

    public void ResetOffset() {
        lastOffset = currentOffset;
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
