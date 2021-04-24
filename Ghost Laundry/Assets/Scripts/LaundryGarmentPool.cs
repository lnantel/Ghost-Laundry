using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryGarmentPool : PoolManager
{
    public GarmentType GarmentPoolType;

    public LaundryGarment GetLaundryGarment(Vector3 position, Transform parent, Garment garment) {
        GameObject obj = SpawnObject(position);
        if(obj != null) {
            LaundryGarment laundryGarment = obj.GetComponent<LaundryGarment>();
            laundryGarment.transform.parent = parent;
            laundryGarment.garment = garment;
            return laundryGarment;
        }

        //In the unlikely event that there are no LaundryGarments left in the pool, instantiate a new one
        Instantiate(garment.laundryGarmentPrefab, position, Quaternion.identity, parent);

        return null;
    }
}
