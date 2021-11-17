using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrant : MonoBehaviour
{
    public bool currentQuad = false;
    public int QuadrantID;
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
    }

    //coroutine for checking if player entered
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            currentQuad = true;
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            currentQuad = false;
            Debug.Log("left quad" + QuadrantID);
        }
    }

    public void SpawnWave(int hunters, int zergs, float pylonHP, GameObject hunter, GameObject zerg)
    {
        gameplayUI.UpdatePylon(pylonHP, QuadrantID); //have to change to List or something
        StartCoroutine(EnemySpawner(0.5f, hunters, hunter, spawnPoints[0], player));
        StartCoroutine(EnemySpawner(0.5f, zergs, zerg, spawnPoints[1], pylon.transform));
    }
    IEnumerator EnemySpawner(float waitTime, int count, GameObject entity, Transform location, Transform target)
    {
        for(int spawned = 0; spawned < count; spawned++)
        {
            Enemy enemy = GameObject.Instantiate(entity, new Vector3(location.position.x, location.position.y, location.position.z), Quaternion.identity).GetComponent<Enemy>(); //use spawnpoints
            enemy.SetTarget(target);
            yield return new WaitForSeconds(waitTime);
        }
    }
    public void Restart()
    {
        StopAllCoroutines();
    }
}
