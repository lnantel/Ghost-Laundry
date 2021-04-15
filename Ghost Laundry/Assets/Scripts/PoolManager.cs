using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public virtual GameObject SpawnObject(Vector3 position) {
        //Return the first inactive object
        for (int i = 0; i < transform.childCount; i++) {
            if (!transform.GetChild(i).gameObject.activeSelf) {
                transform.GetChild(i).position = position;
                transform.GetChild(i).gameObject.SetActive(true);
                return transform.GetChild(i).gameObject;
            }
        }

        //If there are no inactive objects, return the first active object instead
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.activeSelf) {
                transform.GetChild(i).position = position;
                transform.GetChild(i).gameObject.SetActive(false);
                transform.GetChild(i).gameObject.SetActive(true);
                return transform.GetChild(i).gameObject;
            }
        }

        //If there are no objects at all, return null
        Debug.LogError("Object pool is empty.");
        return null;
    }
}
