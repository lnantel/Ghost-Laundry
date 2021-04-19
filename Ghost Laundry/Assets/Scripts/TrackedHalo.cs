using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedHalo : MonoBehaviour
{
    public HaloRenderer Halo;

    private ITrackable Trackable;

    private void Start() {
        Trackable = GetComponent<ITrackable>();
        if(Trackable == null) {
            Debug.LogError("No ITrackable found on this GameObject");
            Destroy(this);
        }
    }

    private void Update() {
        if (Trackable.ContainsTrackedGarment()) {
            if(!Halo.Visible) Halo.ShowHalo();
        }
        else {
            if(Halo.Visible) Halo.HideHalo();
        }
    }
}
