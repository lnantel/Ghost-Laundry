using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaundromatLightingController : MonoBehaviour
{
    public Light2D laundromatLight;
    public Light2D playerLight;
    public Light2D windowLight;
    public Light2D ambientLight;
    public Light2D openSignLight;
    public Light2D signFrameLight;

    public Gradient daylightColor;
    public Gradient windowLightColor;
    public AnimationCurve windowLightIntensity;

    private float windowLightFactor;

    private float time;

    private void Update() {
        time = 1.0f - (TimeManager.instance.RemainingTime() / (TimeManager.instance.RealTimeMinutesPerDay * 60.0f));

        if (CustomerTracker.TrackedCustomer == null) {
            Brighten();
        }
        else {
            Dim();
        }

        if(openSignLight.enabled && time > 10.0f / 12.0f) {
            openSignLight.enabled = false;
            signFrameLight.enabled = false;
        }
        Color currentDaylightColor = daylightColor.Evaluate(time);
        Color currentWindowLightColor = windowLightColor.Evaluate(time);
        windowLight.color = currentWindowLightColor;
        windowLight.intensity = windowLightIntensity.Evaluate(time) * windowLightFactor;
        ambientLight.color = currentDaylightColor;
    }

    public void Dim() {
        windowLightFactor = 1.0f;
        laundromatLight.intensity = 0.0f;
        if (PlayerStateManager.instance.CurrentRoomIndex != 2) playerLight.intensity = 0.8f;
        else playerLight.intensity = 0.0f;
    }

    public void Brighten() {
        if (time < 0.77f) windowLightFactor = 0.25f;
        else windowLightFactor = 0.0f;
        laundromatLight.intensity = 0.8f;
        playerLight.intensity = 0.0f;
    }
}
