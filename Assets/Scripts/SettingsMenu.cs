using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown menuSelect;
    public GameObject avSettings;
    public GameObject inputSettings;
    public Slider sensSlider;
    public TMP_InputField sensInput;
    public Button forwardButton;
    public Button backwardButton;
    public Button leftButton;
    public Button rightButton;
    public Button jumpButton;
    public Button pauseButton;
    public Button fire1Button;
    public Button fire2Button;
    public Button reloadButton;
    public Button fire3Button;
    public TMP_Text forwardText;
    public TMP_Text backwardText;
    public TMP_Text leftText;
    public TMP_Text rightText;
    public TMP_Text jumpText;
    public TMP_Text pauseText;
    public TMP_Text fire1Text;
    public TMP_Text fire2Text;
    public TMP_Text reloadText;
    public TMP_Text fire3Text;
    public GameObject waitForInput;
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityDropdown;
    private int[] keyCodes;
    private void Awake()
    {
        keyCodes = (int[])System.Enum.GetValues(typeof(KeyCode));
    }
    private void Start()
    {

    }
    private void LoadInputSettings()
    {
        ChangeSensInput("" + (PlayerPrefs.GetFloat("sensitivity") / 10));
        ChangeSensSlider(PlayerPrefs.GetFloat("sensitivity"));
        RefreshKeybinds();
        RegisterButtons();
    }
    private void RegisterButtons()
    {
        forwardButton.onClick.AddListener(() => ChangeInputKey("forward"));
        backwardButton.onClick.AddListener(() => ChangeInputKey("backward"));
        leftButton.onClick.AddListener(() => ChangeInputKey("left"));
        rightButton.onClick.AddListener(() => ChangeInputKey("right"));
        jumpButton.onClick.AddListener(() => ChangeInputKey("jump"));
        pauseButton.onClick.AddListener(() => ChangeInputKey("pause"));
        fire1Button.onClick.AddListener(() => ChangeInputKey("fire"));
        fire2Button.onClick.AddListener(() => ChangeInputKey("altfire"));
        reloadButton.onClick.AddListener(() => ChangeInputKey("reload"));
        fire3Button.onClick.AddListener(() => ChangeInputKey("firemode"));
    }
    private void DeRegisterButtons()
    {
        forwardButton.onClick.RemoveListener(() => ChangeInputKey("forward"));
        backwardButton.onClick.RemoveListener(() => ChangeInputKey("backward"));
        leftButton.onClick.RemoveListener(() => ChangeInputKey("left"));
        rightButton.onClick.RemoveListener(() => ChangeInputKey("right"));
        jumpButton.onClick.RemoveListener(() => ChangeInputKey("jump"));
        pauseButton.onClick.RemoveListener(() => ChangeInputKey("pause"));
        fire1Button.onClick.RemoveListener(() => ChangeInputKey("fire"));
        fire2Button.onClick.RemoveListener(() => ChangeInputKey("altfire"));
        reloadButton.onClick.RemoveListener(() => ChangeInputKey("reload"));
        fire3Button.onClick.RemoveListener(() => ChangeInputKey("firemode"));
    }
    private void RefreshKeybinds()
    {
        forwardText.SetText("Forward: " + ((KeyCode)PlayerPrefs.GetInt("forward", (int)KeyCode.W)).ToString());
        backwardText.SetText("Backward: " + ((KeyCode)PlayerPrefs.GetInt("backward", (int)KeyCode.S)).ToString());
        leftText.SetText("Left: " + ((KeyCode)PlayerPrefs.GetInt("left", (int)KeyCode.A)).ToString());
        rightText.SetText("Right: " + ((KeyCode)PlayerPrefs.GetInt("right", (int)KeyCode.D)).ToString());
        jumpText.SetText("Jump: " + ((KeyCode)PlayerPrefs.GetInt("jump", (int)KeyCode.Space)).ToString());
        pauseText.SetText("Pause: " + ((KeyCode)PlayerPrefs.GetInt("pause", (int)KeyCode.Escape)).ToString());
        fire1Text.SetText("Fire: " + ((KeyCode)PlayerPrefs.GetInt("fire", (int)KeyCode.Mouse0)).ToString());
        fire2Text.SetText("Alt Fire: " + ((KeyCode)PlayerPrefs.GetInt("altfire", (int)KeyCode.Mouse1)).ToString());
        reloadText.SetText("Reload: " + ((KeyCode)PlayerPrefs.GetInt("reload", (int)KeyCode.R)).ToString());
        fire3Text.SetText("Fire Mode: " + ((KeyCode)PlayerPrefs.GetInt("firemode", (int)KeyCode.LeftShift)).ToString());
    }
    private void LoadAVSettings()
    {
        qualityDropdown.value = PlayerPrefs.GetInt("quality", 1);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
    }
    public void Quality(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        PlayerPrefs.SetInt("quality", quality);
    }
    public void MenuSelect(int selection)
    {
        PlayerPrefs.SetInt("menuselect", selection);
        if(selection == 0)
        {
            avSettings.SetActive(true);
            inputSettings.SetActive(false);
        }
        else
        {
            avSettings.SetActive(false);
            inputSettings.SetActive(true);
        }
    }
    public void Fullscreen(bool screen)
    {
        Screen.fullScreen = screen;
        if(screen)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }
    }
    private void OnEnable()
    {
        menuSelect.value = PlayerPrefs.GetInt("menuselect", 0);
        MenuSelect(PlayerPrefs.GetInt("menuselect", 0));
        waitForInput.SetActive(false);
        LoadInputSettings();
        LoadAVSettings();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        DeRegisterButtons();
    }
    public void ChangeSensSlider(float newSens)
    {
        if(float.Parse(sensInput.text) != newSens)
        {
            sensInput.text = newSens / 10 + "";
        }
        PlayerPrefs.SetFloat("sensitivity", newSens);
        if (GameManager.manager.IsInGameplay())
        {
            GameManager.manager.player.UpdateSens();
        }
    }
    public void ChangeSensInput(string newSens)
    {
        float sensFloat = float.Parse(newSens);
        int sensInt = Mathf.FloorToInt(sensFloat * 10);
        if (sensSlider.value != sensFloat)
        {
            sensSlider.value = sensInt;
        }
        PlayerPrefs.SetFloat("sensitivity", sensInt);
        if(GameManager.manager.IsInGameplay())
        {
            GameManager.manager.player.UpdateSens();
        }
    }
    public void ChangeInputKey(string key)
    {
        waitForInput.SetActive(true);
        StartCoroutine(NewKeybind(key));
    }
    private IEnumerator NewKeybind(string key)
    {
        yield return new WaitForSeconds(0.0001f);
        bool hasNewKey = false;
        KeyCode oldKey = (KeyCode)PlayerPrefs.GetInt(key);
        KeyCode newKey = (KeyCode)47;
        while (!hasNewKey)
        {
            if (Input.anyKey)
            {
                newKey = GetInput();
            }
            if (newKey != (KeyCode)47)
            {
                InputDuplicateCheck(newKey, oldKey);
                PlayerPrefs.SetInt(key, (int)newKey);
                RefreshKeybinds();
                hasNewKey = true;
            }
            yield return null;
        }
        waitForInput.SetActive(false);
    }
    private KeyCode GetInput()
    {
        for(int keyCode = 0; keyCode < keyCodes.Length; keyCode++)
        {
            if(Input.GetKey((KeyCode)keyCodes[keyCode]))
            {
                return (KeyCode)keyCodes[keyCode];
            }
        }
        return (KeyCode)47;
    }
    private void InputDuplicateCheck(KeyCode key, KeyCode oldKey)
    {
        if(key == (KeyCode)PlayerPrefs.GetInt("forward"))
        {
            PlayerPrefs.SetInt("forward", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("backward"))
        {
            PlayerPrefs.SetInt("backward", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("left"))
        {
            PlayerPrefs.SetInt("left", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("right"))
        {
            PlayerPrefs.SetInt("right", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("jump"))
        {
            PlayerPrefs.SetInt("jump", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("pause"))
        {
            PlayerPrefs.SetInt("pause", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("fire"))
        {
            PlayerPrefs.SetInt("fire", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("altfire"))
        {
            PlayerPrefs.SetInt("altfire", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("reload"))
        {
            PlayerPrefs.SetInt("reload", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
        if (key == (KeyCode)PlayerPrefs.GetInt("firemode"))
        {
            PlayerPrefs.SetInt("firemode", (int)oldKey);
            Debug.Log("Duplicate Input");
        }
    }
    public void LoadMenu()
    {
        GameManager.manager.ReturnToMenu();
        StartCoroutine(LoadScene(1));
    }
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
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
