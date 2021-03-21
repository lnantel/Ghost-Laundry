using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaggerAnimator : MonoBehaviour
{
    public Sprite Open;
    public Sprite Closed;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Closed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            spriteRenderer.sprite = Open;
            AudioManager.instance.PlaySound(Sounds.OpenEmbDoor,0.4f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            spriteRenderer.sprite = Closed;
            AudioManager.instance.PlaySound(Sounds.CloseEmbDoor,0.4f);
        }
    }
}
