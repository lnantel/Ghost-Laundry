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

    public static Action EventManagerReset;

    public GameStates state;
    private IEnumerator stateTransition;

    //This list contains all currently loaded scenes except Main.
    private List<string> loadedScenes;
    private List<string> keepLoaded;
    private IEnumerator scenesLoading;

    private bool inSettings;
    private bool inDialog;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        HideCursor();
        loadedScenes = new List<string>();
        keepLoaded = new List<string>();
        keepLoaded.Add("DebugTools");
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        TimeManager.EndOfDay += OnEndOfDay;
        TransitionManager.TransitionDone += OnTransitionEnd;
        EventManager.StartDialog += OnDialogStart;
        ShowSettings += OnShowSettings;
        HideSettings += OnHideSettings;
        ShowDialog += OnShowDialog;
        HideDialog += OnHideDialog;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        TimeManager.EndOfDay -= OnEndOfDay;
        TransitionManager.TransitionDone -= OnTransitionEnd;
        EventManager.StartDialog -= OnDialogStart;
        ShowSettings -= OnShowSettings;
        HideSettings -= OnHideSettings;
        ShowDialog -= OnShowDialog;
        HideDialog -= OnHideDialog;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.buildIndex != 0 && loadedScenes != null)
            loadedScenes.Add(scene.name);
    }

    private void OnSceneUnloaded(Scene scene) {
        if (scene.buildIndex != 0)
            loadedScenes.Remove(scene.name);
    }

    private bool UnloadAllScenes() {
        if (scenesLoading == null) {
            scenesLoading = UnloadScenesCoroutine();
            StartCoroutine(scenesLoading);
            return true;
        }
        else return false;
    }

    private IEnumerator UnloadScenesCoroutine() {
        List<string> scenesToUnload = new List<string>();
        foreach (string sceneName in loadedScenes) {
            bool keepSceneLoaded = false;
            foreach (string s in keepLoaded) {
                if (s.Equals(sceneName)) keepSceneLoaded = true;
            }
            if (!keepSceneLoaded) {
                scenesToUnload.Add(sceneName);
            }
        }

        foreach(string sceneName in scenesToUnload) {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            while (!op.isDone) yield return null;
        }

        scenesLoading = null;
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
            if(FadeOut != null) FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
            while (scenesLoading != null) yield return null;
        }

        LoadScenes("Title", "Options", "HUD", "Dialog");
        while (scenesLoading != null) yield return null;

        AudioManager.instance.PlayMusic(MusicTrackType.MenuTrack);

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
            if (FadeOut != null) FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
            while (scenesLoading != null) yield return null;
        }

        LoadScenes("HUD", "Laundromat", "Customers", "LaundryTasks", "Pause", "Options", "Shop", "Dialog", "Evaluation", "Day"+TimeManager.instance.CurrentDay);
        while (scenesLoading != null) yield return null;

        SaveManager.LoadSaveData();

        state = GameStates.StartOfDay;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Laundromat"));
        TimeManager.instance.ResetTimeManager();

        if (ShowHUD != null) ShowHUD();
        if (HideSettings != null) HideSettings();
        if (ResumeGame != null) ResumeGame();

        if(TimeManager.instance.CurrentDay != 0) {
            AudioManager.instance.PlayMusic(MusicTrackType.GameplayTrack);

            HideCursor();

            if (FadeIn != null) FadeIn();

            yield return new WaitForSecondsRealtime(1.0f);

            AudioManager.instance.PlaySound(SoundName.LaundromatOpening);

            yield return new WaitForSecondsRealtime(1.0f);
        }
        else {
            AudioManager.instance.PlayMusic(MusicTrackType.TutorialTrack);
        }

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
        if(TimeManager.instance.CurrentDay != 0) {
            //Announce End of Day
            AudioManager.instance.PlaySound(SoundName.LaundromatClosing);
            state = GameStates.EndOfDay;
        }

        //Save progress
        TimeManager.instance.NextDay();
        if (TimeManager.instance.CurrentDay == 1 || MoneyManager.instance.CurrentAmount >= 0) {
            SaveManager.Save();
        }

        if(TimeManager.instance.CurrentDay > 1) {
            //Wait a couple seconds
            yield return new WaitForSecondsRealtime(2.0f);

            //Show the Evaluation screen
            state = GameStates.Evaluation;
            if (ShowEvaluation != null) ShowEvaluation();
            ShowCursor();
            stateTransition = null;
        }
        else {
            stateTransition = GoToTransition();
            StartCoroutine(stateTransition);
        }
    }

    private IEnumerator GoToTransition() {
        if (SceneManager.sceneCount > 1) {
            if (FadeOut != null) FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
            while (scenesLoading != null) yield return null;
        }

        LoadScenes("NextDay", "Options", "HUD", "Dialog");
        while (scenesLoading != null) yield return null;

        SaveManager.LoadSaveData();

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
            if (FadeOut != null) FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
            while (scenesLoading != null) yield return null;
        }

        LoadScenes("SelectionScreen", "Options", "HUD", "Dialog");
        while (scenesLoading != null) yield return null;

        SaveManager.LoadSaveData();

        if (HideHUD != null) HideHUD();
        if (FadeIn != null) FadeIn();
        if (ResumeGame != null) ResumeGame();

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
            if(TimeManager.instance.CurrentDay != 0) {
                stateTransition = GoToTransition();
            }
            else {
                stateTransition = GoToGame();
            }
            StartCoroutine(stateTransition);
        }
    }

    public void LaunchNewGame() {
        SaveManager.CreateNewSave();
        SaveManager.LoadSaveData();
        TimeManager.instance.CurrentDay = 0;
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
        if (inSettings && HideSettings != null) HideSettings();
        else if (!inSettings) {
            if (ResumeGame != null) ResumeGame();
            state = GameStates.Laundromat;
            if(!inDialog) HideCursor();
        }
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

    public void OnDialogStart() {
        if(ShowDialog != null) ShowDialog();
        ShowCursor();
    }
    
    public void OnDialogEnd(int i) {
        if (HideDialog != null) HideDialog();
        HideCursor();
    }

    private void OnShowSettings() {
        inSettings = true;
    }

    private void OnHideSettings() {
        inSettings = false;
    }

    private void OnShowDialog() {
        inDialog = true;
    }

    private void OnHideDialog() {
        inDialog = false;
    }

    public void ShowCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ResetEventManager() {
        StartCoroutine(ReloadDialogScene());
    }

    private IEnumerator ReloadDialogScene() {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Dialog"));
        LoadScenes("Dialog");
        while (scenesLoading != null) yield return null;

        if (EventManagerReset != null) EventManagerReset();
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
