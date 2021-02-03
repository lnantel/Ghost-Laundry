using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkStation : Interactable
{
    public TaskView taskView;
    public LaundryTaskController taskController;
    public GameObject laundryTaskArea;

    private void Start() {
        taskView = FindObjectOfType<TaskView>(true);
    }

    public override void Interact() {
        PlayerController.instance.enabled = false;
        laundryTaskArea.SetActive(true);
        LaundryTaskController task = FindObjectOfType<LaundryTaskController>(true);
        task.gameObject.SetActive(true);
        taskView.PopUp(transform.position);
        LaundryTaskController.exitedTask += OnTaskExit;
    }

    private void OnTaskExit() {
        LaundryTaskController.exitedTask -= OnTaskExit;
        taskView.Minimize(transform.position);
        laundryTaskArea.SetActive(false);
        PlayerController.instance.enabled = true;
    }
}
