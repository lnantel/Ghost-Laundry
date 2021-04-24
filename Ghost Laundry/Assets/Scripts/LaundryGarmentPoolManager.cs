using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryGarmentPoolManager : PoolManager
{
    public static LaundryGarmentPoolManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public LaundryGarmentPool[] LaundryGarmentPools;

    public LaundryGarment SpawnLaundryGarment(Vector3 position, Transform parent, Garment garment) {
        for(int i = 0; i < LaundryGarmentPools.Length; i++) {
            if(LaundryGarmentPools[i].GarmentPoolType == garment.type) {
                LaundryGarment laundryGarment = LaundryGarmentPools[i].GetLaundryGarment(position, parent, garment);
                laundryGarment.UpdateAppearance();
                return laundryGarment;
            }
        }
        return null;
    }

    public void ReturnToPool(LaundryGarment laundryGarment) {
        GarmentType type = laundryGarment.garment.type;

        Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        rb.velocity = Vector3.zero;

        laundryGarment.garment = null;
        for (int i = 0; i < LaundryGarmentPools.Length; i++) {
            if (LaundryGarmentPools[i].GarmentPoolType == type) {
                laundryGarment.transform.parent = LaundryGarmentPools[i].transform;
                laundryGarment.gameObject.SetActive(false);
            }
        }
    }
    public override GameObject SpawnObject(Vector3 position) {
        return base.SpawnObject(position);
    }
}
