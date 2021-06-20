using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text highScoreText;
    private void Start()
    {
        highScoreText.SetText("High score: " + PlayerPrefs.GetInt("highscore", 0));
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadGame()
    {
        StartCoroutine(LoadScene(2));
    }
    public void LoadSettings()
    {
        StartCoroutine(LoadScene(3));
    }
    private IEnumerator LoadScene(int scene)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(scene);
        while(!loading.isDone)
        {
            Debug.Log(loading.progress);
            yield return null;
        }
    }
}
