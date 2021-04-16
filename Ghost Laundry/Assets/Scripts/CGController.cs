using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGController : MonoBehaviour
{
    private Image image;

    private IEnumerator fadeCoroutine;

    private void Start() {
        image = GetComponent<Image>();
    }

    private void OnEnable() {
        GameManager.HideDialog += HideCG;
    }

    private void OnDisable() {
        GameManager.HideDialog -= HideCG;
    }

    public void FadeInCG() {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = Fade(1.0f);
        StartCoroutine(fadeCoroutine);
    }

    public void FadeOutCG() {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = Fade(0.0f);
        StartCoroutine(fadeCoroutine);
    }

    public void ShowCG() {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = Fade(1.0f, false);
        StartCoroutine(fadeCoroutine);
    }

    public void HideCG() {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = Fade(0.0f, false);
        StartCoroutine(fadeCoroutine);
    }

    private IEnumerator Fade(float targetAlpha, bool fade = true) {
        Color color = image.color;
        if (fade) {
            while (color.a != targetAlpha) {
                color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.unscaledDeltaTime);
                image.color = color;
                yield return null;
            }
        }
        color.a = targetAlpha;
        image.color = color;
        fadeCoroutine = null;
    }
}
