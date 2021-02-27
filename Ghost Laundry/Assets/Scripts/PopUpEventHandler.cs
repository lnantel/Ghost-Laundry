using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpEventHandler : MonoBehaviour
{
    public GameObject target;

    public void DestroyTarget() {
        Destroy(target);
    }
}
