using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    public Transform Target;
    public bool Active;

    public GameObject arrow;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        animator = arrow.GetComponent<Animator>();
        spriteRenderer = arrow.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (Target != null) {
            transform.position = Target.position + Vector3.up * 1.5f;
            transform.rotation = Target.rotation;
        }

        if(Target == null && Active) {
            Deactivate();
        }
    }

    public void SetTarget(Transform newTarget) {
        Target = newTarget;
        Activate();
    }

    public void Activate() {
        if(Target != null) {
            Active = true;
            arrow.SetActive(true);
        }
    }

    public void Deactivate() {
        Active = false;
        arrow.SetActive(false);
    }
}
