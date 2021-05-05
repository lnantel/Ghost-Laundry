using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ToastManager : MonoBehaviour
{
    public static ToastManager instance;

    public Flowchart flowchart;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SayLine(string line) {
        flowchart.SetStringVariable("Line", line);
        flowchart.gameObject.SetActive(true);
    }
}
