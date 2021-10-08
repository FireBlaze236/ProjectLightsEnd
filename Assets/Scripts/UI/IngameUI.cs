using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] GameObject _gameOverScreen;

    [SerializeField] GameObject _muteImage;
    bool _muted = false;


    private void OnEnable()
    {
        GameManager.Instance.OnGamePaused += ShowPauseMenu;
        GameManager.Instance.OnGameUnpaused += HidePauseMenu;

        GameManager.Instance.OnGameOver += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGamePaused -= ShowPauseMenu;
        GameManager.Instance.OnGameUnpaused -= HidePauseMenu;

        GameManager.Instance.OnGameOver -= ShowGameOverScreen;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }
    public void RestartGame()
    {
        GameManager.Instance.ReloadThisLevel();
    }
    public void ResumeGame()
    {
        GameManager.Instance.UnpauseGame();
    }
    public void ExitToMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }

    public void ExitGame()
    {
        GameManager.Instance.QuitGame();
    }


    private void ShowPauseMenu()
    {
        _pauseScreen.SetActive(true);
    }

    private void HidePauseMenu()
    {
        _pauseScreen.SetActive(false);
    }

    private void ShowGameOverScreen()
    {

        _gameOverScreen.SetActive(true);
    }

    public void ToggleMuteAudio()
    {
        _muted = !_muted;
        if(_muted)
        {
            _muteImage.gameObject.SetActive(true);
            SFXPlayer.Instance.MuteMusic(_muted);
        }
        else
        {
            _muteImage.gameObject.SetActive(false);
            SFXPlayer.Instance.MuteMusic(_muted);
        }
    }

    
}
