using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject zerg;
    public GameObject hunter;
    public List<Pylon> pylons = new List<Pylon>();
    public Pylon currentPylonTarget;
    public float zergDamage = 10.0f;
    public float hunterDamage = 10.0f;
    public float zergSpeed = 0.01f;
    public float hunterSpeed = 0.05f;
    public int wave = 1;
    public GameplayUIManager ui;
    public float newWaveTimer = 10.0f;
    private int score = 0;
    private int zergCount;
    private int hunterCount;
    private int zergScore;
    private int hunterScore;
    private int waveScore;
    private GameplayUIManager gameplayUI = null;
    private Transform player;
    void Start()
    {
        waveScore = GameManager.manager.waveScore;
        hunterScore = GameManager.manager.hunterScore;
        zergScore = GameManager.manager.zergScore;
        if (GameManager.manager.enemyManager == null && GameManager.manager.enemyManager != this)
        {
            GameManager.manager.enemyManager = this;
            DontDestroyOnLoad(gameObject);
            GameManager.manager.enemyLoaded = true;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
    public void WakeUp()
    {
        wave = 1;
        score = 0;
        UpdateScore(0);
        player = GameManager.manager.player.transform;
        gameplayUI = GameManager.manager.getGameplayUI();
        currentPylonTarget = pylons[0];
        SpawnWave();
    }
    public void SpawnWave()
    {
        gameplayUI.UpdateWave(wave);
        float pylonHP = CalcPylonHp();
        foreach (Pylon pylon in pylons)
        {
            pylon.hp = pylonHP;
        }
        gameplayUI.UpdatePylon1(pylonHP);
        gameplayUI.UpdatePylon2(pylonHP);
        hunterCount = CalcHunterCount();
        SpawnHunters(hunterCount);
        zergCount = CalcZergCount();
        SpawnZerg(zergCount);
        gameplayUI.UpdateEnemies(zergCount + hunterCount);
    }
    private void UpdateScore(int toAdd)
    {
        score += toAdd;
        GameManager.manager.UpdateScore(score);
        Debug.Log(score);
        if(PlayerPrefs.GetInt("highscore", 0) < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }
    public void ZergDied()
    {
        zergCount--;
        gameplayUI.UpdateEnemies(zergCount + hunterCount);
        score += zergScore;
        if(zergCount <= 0 && hunterCount <= 0)
        {
            WaveOver();
        }
    }
    private void WaveOver()
    {
        wave++;
        gameplayUI.UpdateWave(wave);
        UpdateScore(waveScore + Mathf.FloorToInt(currentPylonTarget.hp / 10));
        StartCoroutine(NewWaveTimer());
    }
    private IEnumerator NewWaveTimer()
    {
        yield return new WaitForSeconds(newWaveTimer);
        SpawnWave();
    }
    public void HunterDied()
    {
        hunterCount--;
        gameplayUI.UpdateEnemies(zergCount + hunterCount);
        score += hunterScore;
        if (zergCount <= 0 && hunterCount <= 0)
        {
            WaveOver();
        }
    }
    private void SpawnHunters(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(hunter, new Vector3(100.0f, 1.0f, 80.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(player);
        }
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(hunter, new Vector3(-120.0f, 1.0f, 80.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(player);
        }
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(hunter, new Vector3(100.0f, 1.0f, -100.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(player);
        }
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(hunter, new Vector3(-120.0f, 1.0f, -100.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(player);
        }
    }
    private void SpawnZerg(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(zerg, new Vector3(150.0f, 1.0f, 100.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(currentPylonTarget.transform);
        }
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(zerg, new Vector3(-170.0f, 1.0f, 100.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(currentPylonTarget.transform);
        }
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(zerg, new Vector3(150.0f, 1.0f, -160.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(currentPylonTarget.transform);
        }
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(zerg, new Vector3(-170.0f, 1.0f, -160.0f), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(currentPylonTarget.transform);
        }
    }
    public void Restart()
    {
        StopAllCoroutines();
        pylons = new List<Pylon>();
    }
    private int CalcZergCount()
    {
        return 20 + wave * 2;
    }
    private int CalcHunterCount()
    {
        return 2 + wave;
    }
    private float CalcPylonHp()
    {
        return 100.0f + wave * 10.0f;
    }
}
