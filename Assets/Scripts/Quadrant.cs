﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrant : MonoBehaviour
{
    public bool currentQuad = false;
    public int val;
    public Pylon pylon;
    public List<Transform> spawnPoints;
    private GameplayUIManager gameplayUI = null;
    private Transform player;
    void Awake()
    {
        GameManager.manager.enemyManager.RegisterQuadrant(this);
    }
    public void WakeUp(GameplayUIManager gameUI)
    {
        player = GameManager.manager.player.transform;
        gameplayUI = gameUI;
        Debug.Log("woke up");
    }

    //coroutine for checking if player entered
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            currentQuad = true;
            Debug.Log("entered quad" + val);
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            currentQuad = false;
            Debug.Log("left quad" + val);
        }
    }

    public void SpawnWave(int hunters, int zergs, float pylonHP, GameObject hunter, GameObject zerg)
    {
        gameplayUI.UpdatePylon1(pylonHP); //have to change to List or something
        SpawnHunters(hunters, hunter);
        SpawnZerg(zergs, zerg);
    }
    private void SpawnHunters(int amount, GameObject hunter)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(hunter, new Vector3(spawnPoints[0].position.x, spawnPoints[0].position.y, spawnPoints[0].position.z), Quaternion.identity).GetComponent<Enemy>(); //use spawnpoints
            spawned.SetTarget(player);
        }
    }
    private void SpawnZerg(int amount, GameObject zerg)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy spawned = GameObject.Instantiate(zerg, new Vector3(spawnPoints[1].position.x, spawnPoints[1].position.y, spawnPoints[1].position.z), Quaternion.identity).GetComponent<Enemy>();
            spawned.SetTarget(pylon.transform);
        }
    }
}
