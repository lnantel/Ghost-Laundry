using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnInteract() {
        Debug.Log(gameObject.name + " interaction");
    }

    public virtual void OnGrab() {
        Debug.Log(gameObject.name + " grabbed");
    }

    public virtual void OnRelease() {
        Debug.Log(gameObject.name + " released");
    }
}
