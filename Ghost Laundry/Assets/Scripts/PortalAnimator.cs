using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    private float targetOpacity;

    private void Start() {
        targetOpacity = 0.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
    }

    private void Update() {
        Debug.Log(Vector3.Distance(PlayerStateManager.instance.transform.position, transform.position));
        if (Vector3.Distance(PlayerStateManager.instance.transform.position, transform.position) < 4.0f) {
            targetOpacity = 1.0f;
        }
        else targetOpacity = 0.0f;

        spriteColor.a = Mathf.MoveTowards(spriteColor.a, targetOpacity, 2.0f * Time.deltaTime);
        spriteRenderer.color = spriteColor;
    }
}
