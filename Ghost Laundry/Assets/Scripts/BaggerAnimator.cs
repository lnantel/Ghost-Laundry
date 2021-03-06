﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaggerAnimator : MonoBehaviour
{
    public static Action PlayerNearby;

    public Sprite Open;
    public Sprite Closed;

    public SpriteRenderer CarryableOutline;

    private SpriteRenderer spriteRenderer;
    private bool opened;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Closed;
    }

    private void Update() {
        if (CarryableOutline.enabled && !opened) OpenChute();
        if (!CarryableOutline.enabled && opened) CloseChute();
    }

    private void OpenChute() {
        opened = true;
        spriteRenderer.sprite = Open;
        if (PlayerNearby != null) PlayerNearby();
        AudioManager.instance.PlaySound(SoundName.OpenEmbDoor);
    }

    private void CloseChute() {
        opened = false;
        spriteRenderer.sprite = Closed;
        AudioManager.instance.PlaySound(SoundName.CloseEmbDoor);
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
    //        spriteRenderer.sprite = Open;
    //        if (PlayerNearby != null) PlayerNearby();
    //        AudioManager.instance.PlaySound(SoundName.OpenEmbDoor);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
    //        spriteRenderer.sprite = Closed;
    //        AudioManager.instance.PlaySound(SoundName.CloseEmbDoor);
    //    }
    //}
}
