using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager manager;
    public bool managerLoaded;
    public bool pathfindingLoaded = false;
    public bool enemyLoaded = false;
    public int seed;
    public PathfindingManager pathManager = null;
    public EnemyManager enemyManager = null;
    public UnityEvent getNewPaths = new UnityEvent();
    public UnityEvent restartGame = new UnityEvent();
    public UnityEvent quadrantChange = new UnityEvent();
    public Player player;
    public int zergScore = 1;
    public int hunterScore = 5;
    public int waveScore = 20;
    public float newPathTimer = 3.0f; // onenter game start coroutine with looping timer
    public bool gameOver = false;
    private bool paused = false;
    private bool inGameplay;
    private GameplayUIManager gameUI = null;
    private void Awake()
    {
        managerLoaded = false;
        pathfindingLoaded = false;
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            manager = this;
            seed = 1; //get seed
            Random.InitState(seed);
            Debug.Log(Random.value);
        }
        InitSettings();
        DontDestroyOnLoad(gameObject);
        gameOver = false;
        managerLoaded = true;
        inGameplay = false;
    }
    private void InitSettings()
    {
        PlayerPrefs.SetInt("forward", PlayerPrefs.GetInt("forward", (int)KeyCode.W));
        PlayerPrefs.SetInt("backward", PlayerPrefs.GetInt("backward", (int)KeyCode.S));
        PlayerPrefs.SetInt("left", PlayerPrefs.GetInt("left", (int)KeyCode.A));
        PlayerPrefs.SetInt("right", PlayerPrefs.GetInt("right", (int)KeyCode.D));
        PlayerPrefs.SetInt("jump", PlayerPrefs.GetInt("jump", (int)KeyCode.Space));
        PlayerPrefs.SetInt("pause", PlayerPrefs.GetInt("pause", (int)KeyCode.Escape));
        PlayerPrefs.SetInt("fire", PlayerPrefs.GetInt("fire", (int)KeyCode.Mouse0));
        PlayerPrefs.SetInt("altfire", PlayerPrefs.GetInt("altfire", (int)KeyCode.Mouse1));
        PlayerPrefs.SetInt("reload", PlayerPrefs.GetInt("reload", (int)KeyCode.R));
        PlayerPrefs.SetInt("firemode", PlayerPrefs.GetInt("firemode", (int)KeyCode.LeftShift));
        PlayerPrefs.SetInt("quality", PlayerPrefs.GetInt("quality", 1));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality", 1));
        PlayerPrefs.SetInt("fullscreen", PlayerPrefs.GetInt("fullscreen", 1));
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        PlayerPrefs.SetInt("menuselect", PlayerPrefs.GetInt("menuselect", 0));
    }
    public void OnGameplayStart()
    {
        inGameplay = true;
        player.gameObject.transform.position = new Vector3(11.7f, 0.98f, -61.1f);
        StopCoroutine(PathfindingTimer());
        StartCoroutine(PathfindingTimer());
        enemyManager.WakeUp();
    }
    IEnumerator PathfindingTimer()
    {
        while (inGameplay)
        {
            if(!paused)
            {
                getNewPaths.Invoke();
            }
            yield return new WaitForSeconds(newPathTimer);
        }
    }
    public void UpdateScore(int newVal)
    {
        gameUI.updateScore(newVal);
    }
    public void RegisterGameplayUI(GameplayUIManager gameplayUIManager)
    {
        gameUI = gameplayUIManager;
    }
    public GameplayUIManager getGameplayUI()
    {
        return gameUI;
    }
    public float RandomVal()
    {
        return Random.value;
    }
    public void Lose()
    {
        gameOver = true;
        Pause();
        gameUI.ToggleGameOverUI();
    }
    public void PauseButton()
    {
        gameUI.TogglePauseUI();
        TogglePause();
    }
    private void TogglePause()
    {
        if(paused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }
    private void Pause()
    {
        player.Pause();
        paused = true;
        Time.timeScale = 0.0f;
    }
    private void UnPause()
    {
        player.Pause();
        paused = false;
        Time.timeScale = 1.0f;
    }
    public bool IsNotPaused()
    {
        return !paused;
    }
    public void Restart()
    {
        gameOver = false;
        pathManager.EndPathfinding();
        enemyManager.Restart();
        restartGame.Invoke();
        OnGameplayStart();
    }
    public bool IsInGameplay()
    {
        return inGameplay;
    }
    public void ReturnToMenu()
    {
        if(inGameplay)
        {
            inGameplay = false;
            Time.timeScale = 1.0f;
            StopAllCoroutines();
            pathManager.EndPathfinding();
            enemyManager.Restart();
            Time.timeScale = 1.0f;
            paused = false;
        }
    }
}
