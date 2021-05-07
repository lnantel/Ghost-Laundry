using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMLaundryViewAnimator : MonoBehaviour
{
    public GameObject redLight;
    public GameObject greenLight;

    private WashingMachine washingMachine;

    private Transform animatedTransform;

    private Vector3 idleAnimatedTransformPos;
    private Quaternion idleAnimatedTransformRot;

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    // Start is called before the first frame update
    void Start()
    {
        washingMachine = GetComponentInParent<WashingMachine>();
        animatedTransform = washingMachine.animator.transform;
        idleAnimatedTransformPos = animatedTransform.transform.localPosition;
        idleAnimatedTransformRot = animatedTransform.transform.localRotation;

        initialLocalPos = transform.localPosition;
        initialLocalRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = washingMachine.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("WM_light_off") || stateInfo.IsName("Idle")) {
            greenLight.SetActive(false);
            redLight.SetActive(true);
        }
        else {
            greenLight.SetActive(true);
            redLight.SetActive(false);
        }


        Vector3 localPosOffset = animatedTransform.transform.localPosition - idleAnimatedTransformPos;
        transform.localPosition = initialLocalPos + localPosOffset;
        transform.localRotation = animatedTransform.localRotation;
    }
}
