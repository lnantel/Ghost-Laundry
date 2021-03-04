using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinuteHandAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        float minute = TimeManager.instance.CurrentTime()[1];

        //Convert the minute to a range of 0-1
        minute = minute / 60.0f;

        animator.SetFloat("Hour", minute);
    }
}
