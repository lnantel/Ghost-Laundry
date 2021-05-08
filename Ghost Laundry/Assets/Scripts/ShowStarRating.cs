using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStarRating : MonoBehaviour
{
    public int StarRating;

    public GameObject[] stars;

    public void ShowStars() {
        StartCoroutine(ShowStarsCoroutine());
    }

    private IEnumerator ShowStarsCoroutine() {
        for(int i = 0; i < StarRating && i < stars.Length; i++) {
            stars[i].SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
