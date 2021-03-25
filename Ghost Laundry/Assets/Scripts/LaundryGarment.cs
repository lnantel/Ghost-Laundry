using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class LaundryGarment : LaundryObject
{
    public static Action<LaundryGarment> Created;
    public static Action<LaundryGarment> Released;

    public Garment garment;
    public Sprite[] foldingSprites;
    public Collider2D[] colliders;

    public bool OnFoldingSurface;

    public SortingGroup sortingGroup;

    public SpriteRenderer spriteRenderer;
    public SpriteMask spriteMask;

    public SpriteRenderer fabricRenderer;
    public SpriteRenderer dirtyRenderer;
    public SpriteRenderer wetRenderer;

    public Sprite tornPattern;
    public Sprite meltedPattern;
    public Sprite burnedPattern;

    public ParticleSystem flies;
    public ParticleSystem waterDrops;
    public ParticleSystem sparkles;
    public ParticleSystem ruin;

    private Rigidbody2D rb;
    private LaundryTag laundryTag;
    private bool hovering;
    private bool inspected;
    private Vector2 lastPosition;
    private bool initialized;
    
    private void Start() {
        if(garment == null) {
            garment = Garment.GetRandomGarment();
        }

        sortingGroup = GetComponent<SortingGroup>();

        rb = GetComponent<Rigidbody2D>();
        laundryTag = GetComponentInChildren<LaundryTag>();
        lastPosition = transform.position;
        UpdateAppearance();
        if (Created != null) Created(this);
        initialized = true;
    }

    private void FixedUpdate() {
        lastPosition = rb.position;
    }

    private void LateUpdate() {
        if (!hovering) {
            if(laundryTag != null) laundryTag.Hide();
            inspected = false;
        }
        else hovering = false;
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
                            if (laundryGarment.garment.customerID.Equals(garment.customerID)     //belonging to the same customer
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

    public override void OnGrab() {
        AudioManager.instance.PlaySound(garment.fabric.grabSound);
        if(sortingGroup != null) sortingGroup.sortingOrder = 1;
    }

    public override void OnRelease() {
        if (sortingGroup != null) sortingGroup.sortingOrder = 0;
        rb.velocity = (new Vector2(transform.position.x, transform.position.y) - lastPosition) * 10.0f;

        if(Released != null)
            Released(this);
    }

    private void OnOtherReleased(LaundryGarment other) {
        if(other.GetInstanceID() != GetInstanceID()) {
            if (sortingGroup != null) sortingGroup.sortingOrder--;
        }
    }

    private void OnOtherCreated(LaundryGarment other) {
        if(other.GetInstanceID() != GetInstanceID()) {
            if (sortingGroup != null) sortingGroup.sortingOrder--;
        }
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
        LaundryGarment.Released += OnOtherReleased;
        LaundryGarment.Created += OnOtherCreated;
        if(initialized)
            UpdateAppearance();
    }

    private void OnDisable() {
        LaundryGarment.Released -= OnOtherReleased;
        LaundryGarment.Created -= OnOtherCreated;
    }

    public void UpdateAppearance() {
        if (spriteRenderer != null && foldingSprites != null && spriteMask != null) {
            if (spriteRenderer != null) {
                spriteRenderer.sprite = foldingSprites[garment.currentFoldingStep];
                spriteRenderer.color = garment.color;
            }

            if (spriteMask != null) {
                spriteMask.sprite = foldingSprites[garment.currentFoldingStep];
            }

            if (fabricRenderer != null) {
                fabricRenderer.sprite = garment.fabric.pattern;
                if (garment.Ruined) fabricRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                if (garment.Torn) fabricRenderer.sprite = tornPattern;
                if (garment.Melted) fabricRenderer.sprite = meltedPattern;
                if (garment.Burned) fabricRenderer.sprite = burnedPattern;
            }

            if (dirtyRenderer != null) {
                dirtyRenderer.enabled = !garment.Clean;
            }

            if (flies != null) {
                if (!garment.Clean && !flies.isPlaying) flies.Play();
                if (garment.Clean && flies.isEmitting) flies.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (wetRenderer != null) {
                wetRenderer.enabled = !garment.Dry;
            }

            if (waterDrops != null) {
                if (!garment.Dry && !waterDrops.isPlaying) waterDrops.Play();
                if (garment.Dry && waterDrops.isEmitting) waterDrops.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                ParticleSystem.EmissionModule emission = waterDrops.emission;
                emission.rateOverTime = 8.0f * garment.Humidity;
            }

            if (sparkles != null) {
                if (garment.Pressed && !sparkles.isPlaying){
                sparkles.Play();
                } 
                if (!garment.Pressed && sparkles.isEmitting) sparkles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (garment.Shrunk) {
                spriteRenderer.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
            }
            else {
                spriteRenderer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            if(ruin != null) {
                if (garment.Ruined && !ruin.isPlaying) ruin.Play();
                if (!garment.Ruined && ruin.isEmitting) ruin.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            for (int i = 0; i < colliders.Length; i++) {
                colliders[i].enabled = (i == garment.currentFoldingStep);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        AudioManager.instance.PlaySound(garment.fabric.dropSound);
    }
}
