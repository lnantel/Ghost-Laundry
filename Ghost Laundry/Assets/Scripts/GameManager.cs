﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public static Action ShowSettings;
    public static Action HideSettings;

    public static Action PauseGame;
    public static Action ResumeGame;

    public static Action FadeIn;
    public static Action FadeOut;

    public static Action ShowEvaluation;
    public static Action HideEvaluation;

    public static Action ShowDialog;
    public static Action HideDialog;

    public static Action ShowHUD;
    public static Action HideHUD;

    public GameStates state;
    private IEnumerator stateTransition;

    //This list contains all currently loaded scenes except Main.
    private List<string> loadedScenes;
    private List<string> keepLoaded;
    private IEnumerator scenesLoading;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        HideCursor();
        loadedScenes = new List<string>();
        keepLoaded = new List<string>();
        keepLoaded.Add("HUD");
        keepLoaded.Add("Dialog");
        keepLoaded.Add("Options");
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        TimeManager.EndOfDay += OnEndOfDay;
        TransitionManager.TransitionDone += OnTransitionEnd;
        EventManager.StartDialog += OnDialogStart;
        EventManager.EndDialog += OnDialogEnd;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        TimeManager.EndOfDay -= OnEndOfDay;
        TransitionManager.TransitionDone -= OnTransitionEnd;
        EventManager.StartDialog -= OnDialogStart;
        EventManager.EndDialog -= OnDialogEnd;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.buildIndex != 0)
            loadedScenes.Add(scene.name);
    }

    private void OnSceneUnloaded(Scene scene) {
        if (scene.buildIndex != 0)
            loadedScenes.Remove(scene.name);
    }

    private void UnloadAllScenes() {
        foreach (string sceneName in loadedScenes) {
            bool keepSceneLoaded = false;
            foreach(string s in keepLoaded) {
                if (s.Equals(sceneName)) keepSceneLoaded = true;
            }
            if (!keepSceneLoaded) {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            }
        }
    }

    private bool LoadScenes(params string[] sceneNames) {
        if (scenesLoading == null) {
            scenesLoading = LoadScenesCoroutine(sceneNames);
            StartCoroutine(scenesLoading);
            return true;
        }
        else return false;
    }

    private IEnumerator LoadScenesCoroutine(params string[] sceneNames) {
        yield return new WaitForSecondsRealtime(0.01f);

        List<AsyncOperation> ops = new List<AsyncOperation>();
        foreach(string sceneName in sceneNames) {
            bool alreadyLoaded = false;
            foreach (string s in loadedScenes) {
                if (s.Equals(sceneName)) alreadyLoaded = true;
            }
            if (!alreadyLoaded) {
                AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                op.allowSceneActivation = false;
                ops.Add(op);
                //Wait for each scene to finish loading
                while (op.progress < 0.9f) yield return null;
            }
        }

        //Activate every loading scene
        foreach(AsyncOperation op in ops) {
            op.allowSceneActivation = true;
        }

        //Wait for each scene to activate
        foreach(AsyncOperation op in ops) {
            while (!op.isDone) yield return null;
        }

        scenesLoading = null;
    }

    private IEnumerator GoToTitleScreen() {
        if(SceneManager.sceneCount > 1) {
            FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
        }

        LoadScenes("Title", "Options", "HUD", "Dialog");
        while (scenesLoading != null) yield return null;

        SaveManager.LoadSaveData();

        state = GameStates.TitleScreen;

        if (HideHUD != null) HideHUD();
        if (HideSettings != null) HideSettings();
        if (FadeIn != null) FadeIn();

        ShowCursor();

        stateTransition = null;
    }

    private IEnumerator GoToGame() {
        if (SceneManager.sceneCount > 1) {
            FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
        }

        if (ShowHUD != null) ShowHUD();

        LoadScenes("HUD", "Laundromat", "Customers", "LaundryTasks", "Pause", "Options", "Shop", "Dialog", "Evaluation", "Day"+TimeManager.instance.CurrentDay);
        while (scenesLoading != null) yield return null;

        state = GameStates.StartOfDay;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Laundromat"));

        if (ShowHUD != null) ShowHUD();
        if (HideSettings != null) HideSettings();
        if (ResumeGame != null) ResumeGame();
        if (FadeIn != null) FadeIn();

        HideCursor();

        yield return new WaitForSecondsRealtime(1.0f);

        AudioManager.instance.PlaySound(Sounds.LaundromatOpening);

        yield return new WaitForSecondsRealtime(1.0f);

        TimeManager.instance.StartDay();
        state = GameStates.Laundromat;

        stateTransition = null;
    }

    private void OnEndOfDay(int day) {
        if (stateTransition == null) {
            stateTransition = EndOfDay();
            StartCoroutine(EndOfDay());
        }
    }

    private IEnumerator EndOfDay() {
        //Announce End of Day
        AudioManager.instance.PlaySound(Sounds.LaundromatClosing);
        state = GameStates.EndOfDay;

        //Save progress
        TimeManager.instance.NextDay();
        if (MoneyManager.instance.CurrentAmount >= 0) {
            SaveManager.Save();
        }

        //Wait a couple seconds
        yield return new WaitForSecondsRealtime(2.0f);

        //Show the Evaluation screen
        state = GameStates.Evaluation;
        if(ShowEvaluation != null) ShowEvaluation();
        ShowCursor();

        stateTransition = null;
    }

    private IEnumerator GoToTransition() {
        if (SceneManager.sceneCount > 1) {
            FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
        }

        LoadScenes("NextDay", "Options", "HUD", "Dialog");
        while (scenesLoading != null) yield return null;

        state = GameStates.Transition;

        if (HideHUD != null) HideHUD();
        if (FadeIn != null) FadeIn();

        HideCursor();

        stateTransition = null;
    }

    private void OnTransitionEnd() {
        if (stateTransition == null) {
            stateTransition = GoToGame();
            StartCoroutine(stateTransition);
        }
    }

    private IEnumerator GoToSelectionScreen() {
        if (SceneManager.sceneCount > 1) {
            FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
        }

        LoadScenes("SelectionScreen", "Options", "HUD", "Dialog");
        while (scenesLoading != null) yield return null;

        if (HideHUD != null) HideHUD();
        if (FadeIn != null) FadeIn();

        ShowCursor();

        stateTransition = null;
    }

    private void Update() {
        switch (state) {
            case GameStates.Initialize:
                if (stateTransition == null) {
                    stateTransition = GoToTitleScreen();
                    StartCoroutine(stateTransition);
                }
                break;
            case GameStates.Laundromat:
                if (Input.GetButtonDown("Pause"))
                    Pause();
                break;
            case GameStates.GamePaused:
                if (Input.GetButtonDown("Pause"))
                    Resume();
                break;
            case GameStates.Evaluation:
                break;
            case GameStates.SelectionScreen:
                break;
            default:
                break;
        }
    }

    public void LaunchGame() {
        if(stateTransition == null) {
            stateTransition = GoToTransition();
            StartCoroutine(stateTransition);
        }
    }

    public void LaunchNewGame() {
        SaveManager.CreateNewSave();
        SaveManager.LoadSaveData();
        LaunchGame();
    }
    
    public void LoadGame() {
        if(stateTransition == null) {
            state = GameStates.SelectionScreen;
            stateTransition = GoToSelectionScreen();
            StartCoroutine(stateTransition);
        }
    }

    public void Pause() {
        if(PauseGame != null) PauseGame();
        state = GameStates.GamePaused;
        ShowCursor();
    }

    public void Resume() {
        if(ResumeGame != null) ResumeGame();
        state = GameStates.Laundromat;
        HideCursor();
    }

    public void GoToMainMenu() {
        if (stateTransition == null) {
            stateTransition = GoToTitleScreen();
            StartCoroutine(stateTransition);
        }
    }

    public void OnRetry() {
        if (stateTransition == null) {
            SaveManager.LoadDay(TimeManager.instance.CurrentDay - 1);
            stateTransition = GoToTransition();
            StartCoroutine(stateTransition);
        }
    }

    public void OnNextDay() {
        if (stateTransition == null) {
            stateTransition = GoToTransition();
            StartCoroutine(stateTransition);
        }
    }

    private void OnDialogStart() {
        if(ShowDialog != null) ShowDialog();
        ShowCursor();
    }
    
    private void OnDialogEnd(int i) {
        if (HideDialog != null) HideDialog();
        HideCursor();
    }

    private void ShowCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

public enum GameStates {
    Initialize,
    TitleScreen,
    TitleScreenSettings,
    GamePaused,
    GameSettings,
    Transition,
    Laundromat,
    StartOfDay,
    EndOfDay,
    Evaluation,
    SelectionScreen
}
