using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.ResetGame();
        GameManager.Instance.LoadNextLevel();
    }

    public void ExitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
