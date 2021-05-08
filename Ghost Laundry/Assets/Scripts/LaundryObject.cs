using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryObject : MonoBehaviour
{
    public virtual void OnInteract() {
    }

    public virtual InteractionType GetInteractionType() {
        return InteractionType.None;
    }

    public virtual void OnGrab() {
    }

    public virtual void OnRelease() {
    }

    public virtual void Drag(Vector2 cursorPosition) {
    }

    public virtual void OnInspect() {
    }

    public virtual void OnHover(Vector2 cursorPosition) {
    }
}

public enum InteractionType {
    None,
    Button,
    Open,
    Close,
    Clean,
    Detergent
}
