using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationBarAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        FlyingStar.ReachedDestination += Animate;
    }

    private void OnDisable() {
        FlyingStar.ReachedDestination -= Animate;
    }

    private void Animate(bool sign) {
        animator.ResetTrigger("StarHit");
        animator.SetTrigger("StarHit");
    }
}
