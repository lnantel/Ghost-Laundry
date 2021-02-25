using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryGarment : LaundryObject
{
    public static Action<LaundryGarment> Released;

    public Garment garment;
    public Sprite[] foldingSprites;
    public Collider2D[] colliders;

    public bool OnFoldingSurface;

    public SpriteRenderer fabricRenderer;
    public SpriteRenderer dirtyRenderer;
    public SpriteRenderer wetRenderer;

    public ParticleSystem flies;
    public ParticleSystem waterDrops;
    public ParticleSystem sparkles;

    private Rigidbody2D rb;
    private LaundryTag laundryTag;
    private bool hovering;
    private bool inspected;
    private Vector2 lastPosition;
    private SpriteRenderer spriteRenderer;
    private SpriteMask spriteMask;
    
    private void Start() {
        if(garment == null) {
            garment = Garment.GetRandomGarment();
        }

        rb = GetComponent<Rigidbody2D>();
        laundryTag = GetComponentInChildren<LaundryTag>();
        lastPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMask = GetComponent<SpriteMask>();
        UpdateAppearance();
    }

    private void LateUpdate() {
        if (!hovering) {
            laundryTag.Hide();
            inspected = false;
        }
        else hovering = false;
    }

    private void FixedUpdate() {
        lastPosition = transform.position;
    }

    public LaundryGarment(Garment garment) {
        this.garment = garment;
    }

    public override void OnInteract() {
        inspected = false;
        if (OnFoldingSurface) {
            if(garment is GarmentSock) {
                if(garment.currentFoldingStep == 0) {
                    //Check for a second, nearby, identical sock
                    int layerMask = LayerMask.GetMask("LaundryGarment");
                    Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 0.5f, layerMask);
                    foreach (Collider2D col in cols) {
                        LaundryGarment laundryGarment = col.gameObject.GetComponentInParent<LaundryGarment>();
                        if (laundryGarment != null && laundryGarment.GetInstanceID() == GetInstanceID()) {
                            continue;       //The sock can't pair with itself.
                        }
                        if (laundryGarment != null && laundryGarment.garment is GarmentSock) {                        
                            //If the other garment is also a sock
                            if (laundryGarment.garment.clientName.Equals(garment.clientName)     //belonging to the same customer
                                   && laundryGarment.garment.color.Equals(garment.color)         //of the same color
                                   && laundryGarment.garment.fabric.Equals(garment.fabric)       //and the same fabric
                                   && laundryGarment.garment.currentFoldingStep == 0) {     //and not already paired

                                ((GarmentSock)garment).PairUp((GarmentSock)laundryGarment.garment);
                                Destroy(laundryGarment.gameObject);
                                break;
                            }
                        }
                    }
                }
                else if(garment.currentFoldingStep == 1) {
                    //Separate the socks
                    GarmentSock otherSock = ((GarmentSock)garment).SeparatePair();

                    //Instantiate a LaundryGarment for the other sock
                    LaundryGarment otherLaundryGarment = otherSock.CreateLaundryGarment(transform.position, transform.rotation, transform.parent);
                }
            }
            else {
                garment.Fold();
            }
            UpdateAppearance();
        }
    }

    public override void OnInspect() {
        inspected = !inspected;
    }

    public override void OnHover(Vector2 position) {
        if(laundryTag != null && inspected) {
            laundryTag.Show();
            laundryTag.transform.position = position;
            hovering = true;
        }
    }

    public override void OnRelease() {
        rb.velocity = new Vector2(transform.position.x, transform.position.y) - lastPosition;

        if(Released != null)
            Released(this);
    }

    public override void Drag(Vector2 cursorPosition) {
        inspected = false;
        rb.MovePosition(cursorPosition);
        rb.velocity = Vector3.zero; //Stop gravity from accumulating while the object is grabbed
    }

    public void SetGarment(Garment garment) {
        this.garment = garment;
        UpdateAppearance();
    }

    private void OnEnable() {
        UpdateAppearance();
    }

    private void UpdateAppearance() {
        if (spriteRenderer != null && foldingSprites != null && spriteMask != null) {
            if (spriteRenderer != null) {
                spriteRenderer.sprite = foldingSprites[garment.currentFoldingStep];
                spriteRenderer.color = garment.color;
            }

            if(spriteMask != null) {
                spriteMask.sprite = foldingSprites[garment.currentFoldingStep];
            }

            if(fabricRenderer != null) {
                fabricRenderer.sprite = garment.fabric.pattern;
            }

            if(dirtyRenderer != null) {
                dirtyRenderer.enabled = !garment.Clean;
            }

            if (flies != null) {
                if (!garment.Clean && !flies.isPlaying) flies.Play();
                if (garment.Clean && flies.isEmitting) flies.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if(wetRenderer != null) {
                wetRenderer.enabled = !garment.Dry;
            }

            if (waterDrops != null) {
                if (!garment.Dry && !waterDrops.isPlaying) waterDrops.Play();
                if (garment.Dry && waterDrops.isEmitting) waterDrops.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if(sparkles != null) {
                if (garment.Pressed && !sparkles.isPlaying) sparkles.Play();
                if(!garment.Pressed && sparkles.isEmitting) sparkles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            for (int i = 0; i < colliders.Length; i++) {
                colliders[i].enabled = (i == garment.currentFoldingStep);
            }
        }
    }
}
