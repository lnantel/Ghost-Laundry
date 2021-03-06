﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskView : MonoBehaviour
{
    public static TaskView instance;

    public float popUpTime;
    public float size;

    public bool open;
    private float popUpFactor;
    private float lastPopUpFactor;
    private float timer;
    private Vector3 targetPosition;
    private GameObject taskArea;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        timer = popUpTime;
    }

    public void PopUp(Vector3 fromPosition) {
        timer = 0.0f;
        targetPosition = new Vector3(fromPosition.x, fromPosition.y, -5.0f); ;
        open = true;
    }

    private void Update() {
        timer = Mathf.Clamp(timer + TimeManager.instance.deltaTime, 0.0f, popUpTime);

        if (open)
            popUpFactor = Mathf.Lerp(0.0f, 1.0f, timer / popUpTime);
        else
            popUpFactor = Mathf.Lerp(1.0f, 0.0f, timer / popUpTime);

        if (lastPopUpFactor > 0.0f && popUpFactor == 0.0f) {
            taskArea.SetActive(false);
        }

        transform.localScale = new Vector3(1.6f, 1.0f, 0.9f) * popUpFactor * size;
        transform.position = Vector3.Lerp(targetPosition, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -5.0f), popUpFactor);

        lastPopUpFactor = popUpFactor;
    }

    public void Minimize(Vector3 toPosition, GameObject taskArea) {
        timer = 0.0f;
        targetPosition = new Vector3(toPosition.x, toPosition.y, -5.0f);
        open = false;
        this.taskArea = taskArea;
    }
}
