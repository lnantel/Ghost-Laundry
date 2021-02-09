using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool paused { get; private set; }

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        bool inTitleScreen = SceneManager.GetSceneByName("Title").isLoaded;
        bool inGame = SceneManager.GetSceneByName("Laundromat").isLoaded;
        bool inPauseMenu = SceneManager.GetSceneByName("Pause").isLoaded;
        bool inOptionsMenu = SceneManager.GetSceneByName("Options").isLoaded;

        if (inOptionsMenu) {
            if (Input.GetButtonDown("Pause")) HideOptions();
        }
        else if (inTitleScreen) {

        }
        else if (inGame) {
            if (inPauseMenu) {
                if (Input.GetButtonDown("Pause")) Resume();
            }
            else {
                if (Input.GetButtonDown("Pause")) Pause();
            }
        }
    }

    public void LaunchGame() {
        SceneManager.UnloadSceneAsync("Title");
        SceneManager.LoadSceneAsync("Laundromat", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("LaundryTasks", LoadSceneMode.Additive);
        Time.timeScale = 1.0f;
        HideCursor();
    }

    public void Pause() {
        paused = true;
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
        ShowCursor();
    }

    public void Resume() {
        paused = false;
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("Pause");
        HideCursor();
    }

    public void ShowOptions() {
        SceneManager.LoadSceneAsync("Options", LoadSceneMode.Additive);
    }

    public void HideOptions() {
        SceneManager.UnloadSceneAsync("Options");
    }

    public void GoToMainMenu() {
        Resume();
        SceneManager.UnloadSceneAsync("Laundromat");
        SceneManager.UnloadSceneAsync("LaundryTasks");
        SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
        ShowCursor();
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
