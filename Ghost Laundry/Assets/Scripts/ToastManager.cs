using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ToastManager : MonoBehaviour
{
    public static ToastManager instance;

    public Flowchart bossFlowchart;
    public Flowchart narratorFlowchart;

    public GameObject image;

    private bool DisplayingToast { get => bossFlowchart.gameObject.activeSelf; }
    private List<Toast> Queue;

    private struct Toast {
        public string line;
        public float duration;
        public bool narrator;
    }

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        Queue = new List<Toast>();
    }

    public void SayLine(string line, float duration, bool narrator = false) {
        Toast toast = new Toast();
        toast.line = line;
        toast.duration = duration;
        toast.narrator = narrator;
        AddToastToQueue(toast);
    }

    private void AddToastToQueue(Toast toast) {
        Queue.Add(toast);
        StartNextToast();
    }

    public void StartNextToast() {
        if(!DisplayingToast && Queue.Count > 0) {
            Toast nextToast = Queue[0];
            Queue.RemoveAt(0);
            bossFlowchart.SetStringVariable("Line", nextToast.line);
            bossFlowchart.SetFloatVariable("Duration", nextToast.duration);
            image.SetActive(!nextToast.narrator);
            bossFlowchart.gameObject.SetActive(true);
        }
    }

    public void EndToast() {
        bossFlowchart.gameObject.SetActive(false);
        StartNextToast();
    }
}
