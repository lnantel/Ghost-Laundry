using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerShadow : MonoBehaviour
{
    public Transform DryerRenderer;
    private Vector3 idlePosition;

    private void Start() {
        idlePosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(DryerRenderer.localPosition.x, idlePosition.y, idlePosition.z);
    }
}
