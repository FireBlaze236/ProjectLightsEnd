using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    const int MANAGER_SCENE_INDEX = 1;
    const int PLAYER_SCENE_INDEX = 2;
    const int MENU_SCENE_INDEX = 0;

    

    const int LEVEL_START_INDEX = 3;
    int _currentLevel = -1;


    public Action<float> OnGameLoading;
    public Action OnGamePaused;
    public Action OnGameOver;
    public Action OnGameWin;
    public Action OnGameUnpaused;
    public Action<int> OnStarCollected;

    private bool loading = false;

    public bool isPaused = false;
    public bool isGameover = false;

    public int starsCollected = 0;

    public GameObject spawnPos;

    //THE LOG functions
    public static void Log(string s, int errorType = 0, GameObject origin = null)
    {
        if(Debug.isDebugBuild)
        {
            switch(errorType)
            {
                case 0: Debug.Log(s, origin); break;
                case 1: Debug.LogWarning(s, origin); break;
                case 2: Debug.LogError(s, origin); break;
            }
        }
    }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            StartCoroutine(LoadLevelLoop(MANAGER_SCENE_INDEX, LoadSceneMode.Additive));
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    ReloadThisLevel();
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    LoadNextLevel();
        //}
    }

    IEnumerator LoadLevelLoop(int idx, LoadSceneMode mode)
    {
        loading = true;
        Time.timeScale = 0f;
        AsyncOperation op = SceneManager.LoadSceneAsync(idx, mode);
        while(op.progress <= 0.9f)
        { 
            OnGameLoading?.Invoke(op.progress);
            yield return null;
        }

        Time.timeScale = 1f;

        loading = false;
    }

    public void BackToMainMenu()
    {
        StartCoroutine(LoadLevelLoop(MENU_SCENE_INDEX, LoadSceneMode.Single));

        starsCollected = 0;
    }

    public void LoadNextLevel()
    {
        _currentLevel++;

        if (LEVEL_START_INDEX + _currentLevel >= SceneManager.sceneCountInBuildSettings)
        {
            GameWin();
            return;
        }

        StartCoroutine(LoadLevelLoop(LEVEL_START_INDEX + _currentLevel, LoadSceneMode.Single)); // Load level


        StartCoroutine(LoadLevelLoop(MANAGER_SCENE_INDEX, LoadSceneMode.Additive)); // Load managers

        StartCoroutine(LoadLevelLoop(PLAYER_SCENE_INDEX, LoadSceneMode.Additive));

    }

    public void ReloadThisLevel()
    {
        StartCoroutine(LoadLevelLoop(LEVEL_START_INDEX + _currentLevel, LoadSceneMode.Single)); // Load level


        StartCoroutine(LoadLevelLoop(MANAGER_SCENE_INDEX, LoadSceneMode.Additive)); // Load managers

        StartCoroutine(LoadLevelLoop(PLAYER_SCENE_INDEX, LoadSceneMode.Additive));

        starsCollected = 0;

        isGameover = false;
    }
    


    public void TogglePause()
    {
        if (isPaused)
            UnpauseGame();
        else
            PauseGame();
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (isGameover) return;

        OnGamePaused?.Invoke();
    }
    public void UnpauseGame()
    {
        if (isGameover) return;

        isPaused = false;
        Time.timeScale = 1f;

        OnGameUnpaused?.Invoke();
    }

    public void GameOver()
    {
        
        isGameover = true;
        OnGameOver?.Invoke();

        starsCollected = 0;

        PauseGame();

        //CHECK IF IT IS GREATER THAN THE HIGHEST AMOUNT OF STARS before
    }

    public void GameWin()
    {
        OnGameWin?.Invoke();

        //CHECK IF IT IS GREATER THAN THE HIGHEST AMOUNT OF STARS before
    }

    public void AddStar()
    {
        starsCollected++;
        OnStarCollected?.Invoke(starsCollected);
        
    }

    public void ResetGame()
    {
        isGameover = false;
        starsCollected = 0;
        _currentLevel = -1;
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
