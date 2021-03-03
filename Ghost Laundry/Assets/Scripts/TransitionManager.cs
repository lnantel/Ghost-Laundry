using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransitionManager : MonoBehaviour
{
    public static Action TransitionDone;

    private void Start() {
        StartCoroutine(timer());
    }

    //TODO: Replace this with an animation event when the animation is in place
    private IEnumerator timer() {
        yield return new WaitForSecondsRealtime(3.0f);
        OnTransitionAnimationDone();
    }

    public void OnTransitionAnimationDone() {
        if (TransitionDone != null) TransitionDone();
    }
}
