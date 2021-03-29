using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedHalo : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private Material defaultMaterial;
    private Material haloMaterial;
    private ITrackable Trackable;

    private void Start() {
        Trackable = GetComponent<ITrackable>();
        if(Trackable == null) {
            Debug.LogError("No ITrackable found on this GameObject");
            Destroy(this);
        }

        defaultMaterial = spriteRenderer.material;
        haloMaterial = (Material)Resources.Load("MAT_TrackedHalo");
    }

    private void Update() {
        if (Trackable.ContainsTrackedGarment()) {
            spriteRenderer.material = haloMaterial;
        }
        else {
            spriteRenderer.material = defaultMaterial;
        }
    }
}
