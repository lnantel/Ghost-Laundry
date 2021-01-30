using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerStateManager state;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerStateManager.instance;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Dashing", state.Dashing);
        animator.SetBool("Carrying", state.Carrying);
        animator.SetBool("Walking", state.Walking);
    }
}
