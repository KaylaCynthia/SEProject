using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Icons;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Sprite indo;
    [SerializeField] private Sprite eng;
    [SerializeField] private Image language_button;
    [SerializeField] private Slider bgm;
    [SerializeField] private Slider sfx;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject languagePanel;
    [SerializeField] private GameObject ContinueButton;
    bool languageAktif = true;
    // Start is called before the first frame update
    void Awake()
    {
        //audio settings
        if (!PlayerPrefs.HasKey("bgm") || !PlayerPrefs.HasKey("sfx"))
        {
            PlayerPrefs.SetFloat("bgm", 1);
            PlayerPrefs.SetFloat("sfx", 1);
        }
        if (!PlayerPrefs.HasKey("coins") || !PlayerPrefs.HasKey("day"))
        {
            ContinueButton.SetActive(false);
        }
        bgm.value = PlayerPrefs.GetFloat("bgm");
        sfx.value = PlayerPrefs.GetFloat("sfx");

        settingPanel.SetActive(false);
        languagePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("language") == "Indonesia")
        {
            language_button.sprite = indo;
        }
        else
        {
            language_button.sprite = eng;
        }
        PlayerPrefs.SetFloat("bgm", bgm.value);
        PlayerPrefs.SetFloat("sfx", sfx.value);
        mixer.SetFloat("bgm", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
        mixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20);
    }
    public void startGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void reset_game(GameObject sure)
    {
        if (sure.name == "yes")
        {
            //reset data
            PlayerPrefs.SetFloat("coins", 50000);
            PlayerPrefs.SetInt("day", 1);
            Debug.Log("reset data");
            startGame();
            //GameObject.Find("AreYouSure").SetActive(false);
        }
        else if (sure.activeSelf)
        {
            sure.SetActive(false);
        }
        else
        {
            sure.SetActive(true);
        }
    }
    public void setLanguage(string language)
    {
        if (language != "back")
        {
            PlayerPrefs.SetString("language", language);
            Debug.Log(PlayerPrefs.GetString("language"));
        }
        else
        {
            languagePanel.SetActive(languageAktif);
            languageAktif = !languageAktif;
        }
    }
    public void settingsGame()
    {
        if (settingPanel.activeSelf)
        {
            settingPanel.SetActive(false);
        }
        else
        {
            settingPanel.SetActive(true);
        }
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
