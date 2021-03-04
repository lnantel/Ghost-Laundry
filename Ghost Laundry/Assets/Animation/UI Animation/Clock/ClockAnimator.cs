using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        float timeRemaining = TimeManager.instance.RemainingTime();

        animator.SetFloat("RemainingTime", timeRemaining);
    }
}
