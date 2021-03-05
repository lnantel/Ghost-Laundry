using System.Collections;
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
    private List<Scene> loadedScenes;
    private List<Scene> keepLoaded;
    private IEnumerator scenesLoading;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        HideCursor();
        loadedScenes = new List<Scene>();
        keepLoaded = new List<Scene>();
        //state = GameStates.Initialize;
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
            loadedScenes.Add(scene);
    }

    private void OnSceneUnloaded(Scene scene) {
        if (scene.buildIndex != 0)
            loadedScenes.Remove(scene);
    }

    private void UnloadAllScenes() {
        foreach (Scene scene in loadedScenes) {
            if(!keepLoaded.Contains(scene))
                SceneManager.UnloadSceneAsync(scene.buildIndex);
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
            foreach(Scene scene in loadedScenes) {
                if (scene.name.Equals(sceneName)) {
                    alreadyLoaded = true;
                    break;
                }
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

        LoadScenes("Title", "Options");
        while (scenesLoading != null) yield return null;

        state = GameStates.TitleScreen;

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

        LoadScenes("HUD", "Laundromat", "Customers", "LaundryTasks", "Pause", "Options", "Shop", "Dialog", "Evaluation");
        while (scenesLoading != null) yield return null;

        keepLoaded.Clear();

        state = GameStates.StartOfDay;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Laundromat"));

        if (ShowHUD != null) ShowHUD();
        if (HideSettings != null) HideSettings();
        if (ResumeGame != null) ResumeGame();
        if (FadeIn != null) FadeIn();

        HideCursor();

        yield return new WaitForSecondsRealtime(2.0f);

        TimeManager.instance.StartDay();
        //Play sound
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
        state = GameStates.EndOfDay;

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
            //Unload all scenes except HUD (to preserve resources)
            keepLoaded.Add(SceneManager.GetSceneByName("HUD"));
            keepLoaded.Add(SceneManager.GetSceneByName("Dialog"));
            UnloadAllScenes();
        }

        LoadScenes("NextDay", "HUD", "Dialog");
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
            case GameStates.Evaluation: //TODO: For debug purposes, until the eval screen is done, pressing escape will start the next day
                if (Input.GetButtonDown("Pause")) {
                    if (stateTransition == null) {
                        TimeManager.instance.NextDay();
                        stateTransition = GoToTransition();
                        StartCoroutine(stateTransition);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void LaunchGame() {
        if(state == GameStates.TitleScreen && stateTransition == null) {
            //stateTransition = GoToGame();
            stateTransition = GoToTransition();
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
        //TODO: Save system: reset resources/narrative events to their state at the start of the day
        if (stateTransition == null) {
            TimeManager.instance.RetryDay();
            stateTransition = GoToTransition();
            StartCoroutine(stateTransition);
        }
    }

    public void OnNextDay() {
        if (stateTransition == null) {
            TimeManager.instance.NextDay();
            stateTransition = GoToTransition();
            StartCoroutine(stateTransition);
        }
    }

    private void OnDialogStart() {
        if(ShowDialog != null) ShowDialog();
    }
    
    private void OnDialogEnd(int i) {
        if (HideDialog != null) HideDialog();
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
    Evaluation
}
