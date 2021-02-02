using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryObject : MonoBehaviour
{
    public virtual void OnInteract() {
        Debug.LogWarning(gameObject.name + " interaction not implemented");
    }

    public virtual void OnGrab() {
        Debug.LogWarning(gameObject.name + " grab not implemented");
    }

    public virtual void OnRelease() {
        Debug.LogWarning(gameObject.name + " release not implemented");
    }

    public virtual void Drag(Vector2 cursorPosition) {
        transform.position = cursorPosition;
    }
}
