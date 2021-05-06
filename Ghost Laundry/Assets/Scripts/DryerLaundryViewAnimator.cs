using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerLaundryViewAnimator : MonoBehaviour
{
    private Dryer dryer;

    private Transform animatedTransform;

    private Vector3 idleAnimatedTransformPos;
    private Quaternion idleAnimatedTransformRot;

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    public GameObject greenLight;

    // Start is called before the first frame update
    void Start() {
        dryer = GetComponentInParent<Dryer>();

        animatedTransform = dryer.animator.transform;
        idleAnimatedTransformPos = animatedTransform.transform.localPosition;
        idleAnimatedTransformRot = animatedTransform.transform.localRotation;

        initialLocalPos = transform.localPosition;
        initialLocalRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update() {
        AnimatorStateInfo stateInfo = dryer.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("D_light_off") || stateInfo.IsName("Idle")) {
            greenLight.SetActive(false);
        }
        else {
            greenLight.SetActive(true);
        }

        Vector3 localPosOffset = animatedTransform.transform.localPosition - idleAnimatedTransformPos;
        transform.localPosition = initialLocalPos + localPosOffset;
        transform.localRotation = animatedTransform.localRotation;
    }
}
