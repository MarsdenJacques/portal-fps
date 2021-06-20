using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("game scene start");
        GameManager.manager.OnGameplayStart();
    }
}
