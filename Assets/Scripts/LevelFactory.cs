using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//maybe use a seed

public class LevelFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject[] obstaclePrefabs;
    [SerializeField]
    private int minQuadrants = 4;
    [SerializeField]
    private int maxQuadrants = 4;
    [SerializeField]
    private int minObstacles = 10;
    [SerializeField]
    private int maxObstacles = 40;
    [SerializeField]
    private GameObject quadrant;
    [SerializeField]
    private GameObject levelPrefab;
    private int width;
    private int height;
    private int depth;
    private float garbage;
    void Start()
    {
        if (GameManager.manager.levelFactory == null && GameManager.manager.levelFactory != this)
        {
            GameManager.manager.levelFactory = this;
            DontDestroyOnLoad(gameObject);
            GameManager.manager.factoryLoaded = true;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
    public Level RequestLevel(int quadCount)
    {
        //make coroutine and show progress on loading screen
        return InitLevel(quadCount);
    }
    //quadCount of 0 means random
    private Level InitLevel(int quadCount)
    {
        Level level = GameObject.Instantiate(levelPrefab).GetComponent<Level>();
        Quadrant[] initQuadrants = new Quadrant[maxQuadrants];
        for (int quad = 0; quad < initQuadrants.Length; quad++)
        {
            initQuadrants[quad] = genQuad(level.gameObject);
        }
        if (quadCount == 0)
        {
            quadCount = Mathf.RoundToInt(Random.Range(minQuadrants, 4.01f));// maxQuadrants));
        }
        else
        {
            //throwing out the unneeded quadcount roll from the current seed
            garbage = Random.Range(minQuadrants, 4.01f);// maxQuadrants));
        }
        //only using the quadrants required
        Quadrant[] quadrants = new Quadrant[quadCount];
        for(int quad = 0; quad < initQuadrants.Length; quad++)
        {
            if(quad < quadCount)
            {
                quadrants[quad] = initQuadrants[quad];
            }
            else
            {
                GameObject.Destroy(initQuadrants[quad].gameObject);
            }
        }
        level.SetQuadrants(quadrants);
        return level;
    }
    private Quadrant genQuad(GameObject level)
    {
        int obstacleCount = Mathf.RoundToInt(Random.Range(minObstacles, maxObstacles));
        GameObject newQuadrant = GameObject.Instantiate(quadrant);//need to set placement later
        newQuadrant.transform.SetParent(level.transform);
        for(int obstacle = 0; obstacle < obstacleCount; obstacle++)
        {
            int obstacleType = Mathf.RoundToInt(Random.Range(0.0f, obstaclePrefabs.Length - 1));
            GameObject newObstacle = GameObject.Instantiate(obstaclePrefabs[obstacleType]);
            newObstacle.transform.SetParent(newQuadrant.transform);//need to set placement later
        }
        newQuadrant.SetActive(false);
        return newQuadrant.GetComponent<Quadrant>();
    }
}
