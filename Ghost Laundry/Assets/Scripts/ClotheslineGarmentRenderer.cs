using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClotheslineGarmentRenderer : MonoBehaviour
{
    public Garment garment;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer fabricRenderer;
    [SerializeField]
    private SpriteRenderer dirtyRenderer;
    [SerializeField]
    private SpriteRenderer wetRenderer;

    //[SerializeField]
    //private ParticleSystem flies;
    [SerializeField]
    private ParticleSystem drops;
    [SerializeField]
    private ParticleSystem sparkles;
    [SerializeField]
    private ParticleSystem ruin;

    [SerializeField]
    private Sprite tornPattern;
    [SerializeField]
    private Sprite meltedPattern;
    [SerializeField]
    private Sprite burnedPattern;

    private void Update() {
        if(garment != null) {
            if (!garment.Dry) {
                ParticleSystem.EmissionModule emission = drops.emission;
                emission.rateOverTimeMultiplier = 8.0f * garment.Humidity;
            }
            else {
                wetRenderer.enabled = false;
                drops.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private void OnEnable() {
        if(garment != null) {
            UpdateAppearance();
        }
    }

    public void UpdateAppearance() {
        //Update appearance
        if (garment != null) {
            spriteRenderer.color = garment.color;

            if (!garment.Clean) {
                dirtyRenderer.enabled = true;
                //flies.Play();
            }
            else {
                dirtyRenderer.enabled = false;
                //flies.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (!garment.Dry) {
                wetRenderer.enabled = true;
                drops.Play();
            }
            else {
                wetRenderer.enabled = false;
                drops.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (garment.Ruined) {
                ruin.Play();
            }
            else {
                ruin.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (garment.Pressed) {
                sparkles.Play();
            }
            else {
                sparkles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            fabricRenderer.sprite = garment.fabric.pattern;
            if (garment.Torn) fabricRenderer.sprite = tornPattern;
            if (garment.Melted) fabricRenderer.sprite = meltedPattern;
            if (garment.Burned) fabricRenderer.sprite = burnedPattern;
        }
    }
}
