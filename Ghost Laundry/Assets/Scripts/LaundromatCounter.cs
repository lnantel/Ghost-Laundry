using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundromatCounter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundromatObject")) {
            LaundromatSpriteSort spriteSort = collision.GetComponent<LaundromatSpriteSort>();
            spriteSort.forceToFront = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundromatObject") && collision.enabled == true) {
            LaundromatSpriteSort spriteSort = collision.GetComponent<LaundromatSpriteSort>();
            spriteSort.forceToFront = false;
        }
    }
}
