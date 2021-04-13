using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaggerAnimator : MonoBehaviour
{
    public static Action PlayerNearby;

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
            if (PlayerNearby != null) PlayerNearby();
            AudioManager.instance.PlaySound(SoundName.OpenEmbDoor,0.4f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            spriteRenderer.sprite = Closed;
            AudioManager.instance.PlaySound(SoundName.CloseEmbDoor,0.4f);
        }
    }
}
