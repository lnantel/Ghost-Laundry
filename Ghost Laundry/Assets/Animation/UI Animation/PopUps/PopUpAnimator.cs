using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnimator : MonoBehaviour
{
    public GameObject target;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void AnimationDone() {
        target.SetActive(false);
        animator.ResetTrigger("HidePopUp");
    }
}
