using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionManager : MonoBehaviour
{
    public static Action ShowConfirmationWindow;
    public static Action HideConfirmationWindow;

    public Transform ghost;

    public Transform[] Levels;

    public GameObject confirmationWindow;

    public float ghostSpeed;

    private Animator ghostAnimator;

    private int selectedDay;
    private int ghostDestination;

    private bool initialized;

    private void OnEnable() {
        LevelTileSelector.DaySelected += OnDaySelected;
        LevelTileSelector.DayClicked += OnDayClicked;
    }

    private void OnDisable() {
        LevelTileSelector.DaySelected -= OnDaySelected;
        LevelTileSelector.DayClicked -= OnDayClicked;
    }

    private void Start() {
        ghostAnimator = ghost.GetComponentInChildren<Animator>();
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        while (TimeManager.instance == null)
            yield return null;
        yield return new WaitForSecondsRealtime(0.1f);

        int CurrentDay = TimeManager.instance.CurrentDay;

        for (int i = 0; i < Levels.Length; i++) {
            if (i <= CurrentDay) Levels[i].gameObject.GetComponent<LevelTileAnimator>().Flip();
        }

        selectedDay = CurrentDay;
        ghostDestination = CurrentDay;
        ghost.transform.position = Levels[ghostDestination].position;
        initialized = true;
    }

    private void Update() {
        if (initialized && MoveTowards(ghostDestination)) {
            if (ghostDestination < selectedDay) ghostDestination++;
            if (ghostDestination > selectedDay) ghostDestination--;
        }
    }

    private bool MoveTowards(int destination) {
        ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, Levels[destination].position, ghostSpeed * Time.deltaTime);
        bool arrived = Vector3.Distance(ghost.transform.position, Levels[destination].position) < 0.1f;
        ghostAnimator.SetBool("Walking", !arrived);
        return arrived;
    }

    private void OnDaySelected(int day) {
        selectedDay = day;
    }

    private void OnDayClicked(int day) {
        if (ShowConfirmationWindow != null) ShowConfirmationWindow();
        confirmationWindow.SetActive(true);
    }

    public void OnSelectionConfirmed() {
        SaveManager.LoadDay(selectedDay);
        GameManager.instance.LaunchGame();
    }

    public void OnSelectionCancelled() {
        if (HideConfirmationWindow != null) HideConfirmationWindow();
        confirmationWindow.SetActive(false);
    }

    public void BackToMainMenu() {
        GameManager.instance.GoToMainMenu();
    }
}
