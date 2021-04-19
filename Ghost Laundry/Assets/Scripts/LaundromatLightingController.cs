using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaundromatLightingController : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerLight;

    private void Update() {
        if(CustomerTracker.TrackedCustomer == null) {
            Brighten();
        }
        else {
            Dim();
        }
    }

    public void Dim() {
        globalLight.intensity = 0.22f;
        playerLight.intensity = 0.9f;
    }

    public void Brighten() {
        globalLight.intensity = 1.0f;
        playerLight.intensity = 0.0f;
    }
}
