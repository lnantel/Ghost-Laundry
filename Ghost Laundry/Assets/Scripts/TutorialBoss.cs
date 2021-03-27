using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoss : Interactable
{
    public TutorialManager tutorialManager;

    protected override void Start() {
        popUpPrefab = (GameObject)Resources.Load("DialogPopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position, transform.rotation, transform);
        popUpInstance.SetActive(false);
    }

    public override void Interact() {
        tutorialManager.OnReady();
    }
}
