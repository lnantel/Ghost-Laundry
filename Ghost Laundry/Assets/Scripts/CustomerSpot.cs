using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CustomerSpot
{
    public static Action<CustomerSpot> Freed;

    [HideInInspector]
    public int ID;

    [HideInInspector]
    public Customer customer;

    public Vector3 position;
    public bool Claimed { get => customer != null; }

    public void Free() {
        customer = null;
        if (Freed != null) Freed(this);
    }

    public bool Equals(CustomerSpot other) {
        return ID == other.ID;
    }
}
