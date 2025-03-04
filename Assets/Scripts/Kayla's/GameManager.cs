using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    //audio settings
    [SerializeField] private Slider bgm;
    [SerializeField] private Slider sfx;
    [SerializeField] private AudioMixer mixer;

    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI DayText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    private int currentDay = 1;
    private float currentTime = 540f; // 09:00 in minutes (9 * 60)
    private float endTime = 1260f; // 21:00 in minutes (21 * 60)
    private float timeScale = 5f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //audio settings
        if (!PlayerPrefs.HasKey("bgm") || !PlayerPrefs.HasKey("sfx"))
        {
            PlayerPrefs.SetFloat("bgm", 1);
            PlayerPrefs.SetFloat("sfx", 1);
        }
        bgm.value = PlayerPrefs.GetFloat("bgm");
        sfx.value = PlayerPrefs.GetFloat("sfx");

        UpdateDayText();
        UpdateTimer();
    }

    public void OpenPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void Update()
    {
        //audio settings
        PlayerPrefs.SetFloat("bgm", bgm.value);
        PlayerPrefs.SetFloat("sfx", sfx.value);
        mixer.SetFloat("bgm", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
        mixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20);

        currentTime += Time.deltaTime / timeScale * 15;

        if (currentTime >= endTime)
        {
            currentTime = 540f;
            currentDay++;
            UpdateDayText();
        }

        UpdateTimer();
    }

    private void UpdateDayText()
    {
        if (!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "Indonesia")
        {
            DayText.text = "Hari: " + currentDay.ToString();
        }
        else if (PlayerPrefs.GetString("language") == "English")
        {
            DayText.text = "Day: " + currentDay.ToString();
        }
    }

    private void UpdateTimer()
    {
        float roundedTime = Mathf.Round(currentTime / 15) * 15;
        if (Mathf.Abs(currentTime - roundedTime) < 0.1f)
        {
            int hours = Mathf.FloorToInt(roundedTime / 60);
            int minutes = Mathf.FloorToInt(roundedTime % 60);
            TimerText.text = string.Format("{0:00}:{1:00}", hours, minutes);
        }
    }
}
