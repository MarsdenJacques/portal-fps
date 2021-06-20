using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("LoadGame");
    }
    IEnumerator LoadGame()
    {
        while(!GameManager.manager.managerLoaded || !GameManager.manager.pathfindingLoaded || !GameManager.manager.enemyLoaded)
        {
            yield return null;
        }
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(1);
        while(!loadingScene.isDone)
        {
            Debug.Log(loadingScene.progress);
            yield return null;
        }
    }
}
