using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupMenu : MonoBehaviour
{
    private bool _paused = false;

    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject DeathPanel; //Maybe not the best name?

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            PauseGame();
        }

    }

    public void PauseGame() {
        PausePanel.SetActive(!PausePanel.activeSelf);
        _paused = !_paused;

        if (_paused) {
            Time.timeScale = 0f;
        }
        else {
            Time.timeScale = 1f;
        }
    }

    public void DeathPopup() {
        DeathPanel.SetActive(!PausePanel.activeSelf);
        _paused = !_paused;

        if (_paused) {
            Time.timeScale = 0f;
        }
        else {
            Time.timeScale = 1f;
        }
    }

    public void MainMenu() {
        Time.timeScale = 1f; //TODO: make a scene manager to prevent having to do this?
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
