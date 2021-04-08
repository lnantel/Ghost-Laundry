using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerStateManager state;
    private Animator animator;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerStateManager.instance;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        yield return null;
        SetCap();
    }

    private void OnEnable() {
        GameManager.HideDialog += SetCap;
    }

    private void OnDisable() {
        GameManager.HideDialog -= SetCap;
    }

    private void SetCap() {
        bool cap = EventManager.instance.OlliesCap();
        float layerWeight = cap ? 1.0f : 0.0f;
        animator.SetLayerWeight(animator.GetLayerIndex("CapLayer"), layerWeight);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.flipX = PlayerController.instance.facingRight;
        animator.SetBool("Dashing", state.Dashing);
        animator.SetBool("Carrying", state.Carrying);
        animator.SetBool("Walking", state.Walking);
    }
}
