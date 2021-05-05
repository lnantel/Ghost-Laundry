using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ToastManager : MonoBehaviour
{
    public static ToastManager instance;

    public Flowchart flowchart;

    private bool DisplayingToast { get => flowchart.gameObject.activeSelf; }
    private List<Toast> Queue;

    private struct Toast {
        public string line;
        public float duration;
    }

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        Queue = new List<Toast>();
    }

    public void SayLine(string line, float duration) {
        Toast toast = new Toast();
        toast.line = line;
        toast.duration = duration;
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
            flowchart.SetStringVariable("Line", nextToast.line);
            flowchart.SetFloatVariable("Duration", nextToast.duration);
            flowchart.gameObject.SetActive(true);
        }
    }

    public void EndToast() {
        flowchart.gameObject.SetActive(false);
        StartNextToast();
    }
}
