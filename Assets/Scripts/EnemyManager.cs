﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject zerg;
    public GameObject hunter;
    public List<Quadrant> quadrants = new List<Quadrant>();
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
        gameplayUI = GameManager.manager.getGameplayUI();
        foreach(Quadrant quadrant in quadrants)
        {
            quadrant.WakeUp(gameplayUI);
        }
        SpawnWave();
    }
    public void SpawnWave()
    {
        gameplayUI.UpdateWave(wave);
        float pylonHP = CalcPylonHp();
        hunterCount = CalcHunterCount();
        zergCount = CalcZergCount();
        foreach(Quadrant quadrant in quadrants)
        {
            quadrant.SpawnWave(hunterCount, zergCount, pylonHP, hunter, zerg);
        }
        gameplayUI.UpdateEnemies(zergCount*quadrants.Count + hunterCount*quadrants.Count);
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
        int pylonScore = 0;
        foreach(Quadrant quadrant in quadrants)
        {
            pylonScore += Mathf.FloorToInt(quadrant.pylon.hp / 10);
        }
        UpdateScore(waveScore + pylonScore);
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
    public void Restart()
    {
        StopAllCoroutines();
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
