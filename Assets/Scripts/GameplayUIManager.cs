using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText = null;
    [SerializeField]
    private TMP_Text waveText = null;
    [SerializeField]
    private TMP_Text enemyText = null;
    [SerializeField]
    private TMP_Text pylon1Text = null;
    [SerializeField]
    private TMP_Text pylon2Text = null;
    [SerializeField]
    private TMP_Text pylon3Text = null;
    [SerializeField]
    private TMP_Text pylon4Text = null;
    [SerializeField]
    private TMP_Text ammoText = null;
    [SerializeField]
    private Slider healthSlider = null;
    [SerializeField]
    private GameObject pauseUI = null;
    [SerializeField]
    private GameObject settingsUI = null;
    [SerializeField]
    private GameObject gameOverUI = null;

    private void Start()
    {
        GameManager.manager.RegisterGameplayUI(this);
        scoreText.SetText("Score: " + 0);
    }

    public void updateScore(int newVal)
    {
        scoreText.SetText("Score: " + newVal);
    }
    public void UpdateAmmo(int newVal)
    {
        ammoText.SetText(""+newVal);
    }
    public void UpdatePylon(float newVal, int pylonId)
    {
        switch (pylonId)
        {
            case 1:
                UpdatePylon1(newVal);
                break;
            case 2:
                UpdatePylon2(newVal);
                break;
            case 3:
                UpdatePylon3(newVal);
                break;
            case 4:
                UpdatePylon4(newVal);
                break;
            default:
                Debug.Log("Invalid pylon ID");
                break;
        }
    }
    private void UpdatePylon1(float newVal)
    {
        pylon1Text.SetText("Pylon 1 HP: " + newVal);
    }
    private void UpdatePylon2(float newVal)
    {
        pylon2Text.SetText("Pylon 2 HP: " + newVal);
    }
    private void UpdatePylon3(float newVal)
    {
        pylon3Text.SetText("Pylon 3 HP: " + newVal);
    }
    private void UpdatePylon4(float newVal)
    {
        pylon4Text.SetText("Pylon 4 HP: " + newVal);
    }
    public void UpdateEnemies(int newVal)
    {
        enemyText.SetText("Enemy Count: " + newVal);
    }
    public void UpdateWave(int newVal)
    {
        waveText.SetText("Wave: " + newVal);
    }
    public void UpdateHP(float newVal)
    {
        healthSlider.value = newVal;
    }
    public void TogglePauseUI()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
    public void ToggleGameOverUI()
    {
        pauseUI.SetActive(true);
        gameOverUI.SetActive(true);
    }
    public void Restart()
    {
        gameOverUI.SetActive(false);
        pauseUI.SetActive(false);
        GameManager.manager.Restart();
    }
}
