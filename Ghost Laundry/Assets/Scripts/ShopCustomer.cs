using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCustomer : MonoBehaviour
{
    public Sprite[] CustomerSprites;

    public float lifetime;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Initialize();
    }

    private void OnEnable() {
        Initialize();
    }

    private void Initialize() {
        if(spriteRenderer != null) {
            spriteRenderer.sprite = CustomerSprites[UnityEngine.Random.Range(0, CustomerSprites.Length)];
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn() {
        float alpha = 0.0f;
        Color spriteColor = spriteRenderer.color;
        while (alpha < 0.6f) {
            alpha = Mathf.MoveTowards(alpha, 0.6f, TimeManager.instance.deltaTime / 1.5f);
            spriteColor.a = alpha;
            spriteRenderer.color = spriteColor;
            yield return null;
        }
        StartCoroutine(LifetimeCoroutine());
    }

    public void Despawn() {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        float alpha = 0.6f;
        Color spriteColor = spriteRenderer.color;
        while (alpha > 0.0f) {
            alpha = Mathf.MoveTowards(alpha, 0.0f, TimeManager.instance.deltaTime / 1.5f);
            spriteColor.a = alpha;
            spriteRenderer.color = spriteColor;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private IEnumerator LifetimeCoroutine() {
        yield return new WaitForLaundromatSeconds(lifetime);
        StartCoroutine(FadeOut());
    }
}
