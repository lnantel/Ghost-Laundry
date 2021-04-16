using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EpilogueManager : MonoBehaviour
{
    public static Action StartEpilogue;

    private void Start() {
        StartCoroutine(OnEpilogueStart());
    }

    private IEnumerator OnEpilogueStart() {
        yield return new WaitForSecondsRealtime(0.1f);
        if (StartEpilogue != null) StartEpilogue();
    }
}
