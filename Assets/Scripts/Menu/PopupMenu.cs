using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour
{
    private bool _paused = false;

    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject DeathPanel; //Maybe not the best name?

    [SerializeField]
    private GameObject LevelClearedPanel; //Maybe not the best name?

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            PauseGame();
        }
    }

    public void DeathPopup() {
        DeathPanel.SetActive(!DeathPanel.activeSelf);
        var audioSource = DeathPanel.GetComponentInChildren<AudioSource>();
        audioSource.Play();

        PauseGameTime();
    }

    public void LevelCleared() {
        LevelClearedPanel.SetActive(!LevelClearedPanel.activeSelf);
        var audioSource = LevelClearedPanel.GetComponentInChildren<AudioSource>();
        audioSource.Play();
        PauseGameTime();

        var currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if(SceneManager.sceneCountInBuildSettings -1 <= currentBuildIndex) {
            var buttons = LevelClearedPanel.GetComponentsInChildren<Button>();
            for(int i = 0; i < buttons.Length; i++) {
                if(buttons[i].name == "NextLevel") {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void MainMenu() {
        Time.timeScale = 1f; //TODO: make a scene manager to prevent having to do this?
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }


    public void PauseGame() {
        PausePanel.SetActive(!PausePanel.activeSelf);
        var audioSource = PausePanel.GetComponentInChildren<AudioSource>();
        audioSource.Play();

        PauseGameTime();
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PauseGameTime() {
        _paused = !_paused;

        if (_paused) {
            Time.timeScale = 0f;
        }
        else {
            Time.timeScale = 1f;
        }
    }
}
