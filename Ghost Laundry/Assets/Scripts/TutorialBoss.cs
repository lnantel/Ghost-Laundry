using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoss : Interactable
{
    public TutorialManager tutorialManager;
    public Transform popUpPos;

    protected override void Start() {
        popUpPrefab = (GameObject)Resources.Load("DialogPopUp");
        popUpInstance = Instantiate(popUpPrefab, popUpPos.position, popUpPos.rotation, transform);
        popUpAnimator = popUpInstance.GetComponentInChildren<Animator>();
        popUpInstance.SetActive(false);
    }

    protected override void Interaction() {
        tutorialManager.BossDialog();
    }
}
