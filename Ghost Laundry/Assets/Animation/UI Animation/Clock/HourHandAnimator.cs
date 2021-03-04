using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourHandAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        float hour = TimeManager.instance.CurrentTime()[0];
        float minute = TimeManager.instance.CurrentTime()[1];

        //Convert the hour to a range of 0-1
        hour = (hour - 12.0f) / 12.0f;
        hour += (minute / 60.0f) / 12.0f;
        animator.SetFloat("Hour", hour);
    }
}
