using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider bgm;
    [SerializeField] private Slider sfx;
    [SerializeField] private AudioMixer mixer;

    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI DayText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject receiptPanel;
    [SerializeField] private GameObject customerSpawner;
    [SerializeField] private Transform customerParent;

    private int currentDay = 1;
    private float currentTime = 540f; // 09:00 in minutes (9 * 60)
    private float endTime = 1260f; // 21:00 in minutes (21 * 60)
    private float timeScale = 5f;
    private bool isShopClosed = false;

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


    public bool isPaused = false;
    public void Pause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        Time.timeScale = isPaused ? 0 : 1;
    }

    bool isSettings = false;
    public void OpenSettings()
    {
        isSettings = !isSettings;
        pausePanel.SetActive(!isSettings);
        settingsPanel.SetActive(isSettings);
    }


    private void Update()
    {
        PlayerPrefs.SetFloat("bgm", bgm.value);
        PlayerPrefs.SetFloat("sfx", sfx.value);
        mixer.SetFloat("bgm", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
        mixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20);

        if (!isShopClosed)
        {
            currentTime += Time.deltaTime / timeScale * 15;

            if (currentTime >= endTime)
            {
                CloseShop();
            }
        }
        else
        {
            if (customerParent.childCount == 0)
            {
                ShowReceipt();
            }
        }

        UpdateTimer();
    }

    private void CloseShop()
    {
        isShopClosed = true;
        customerSpawner.SetActive(false);
    }

    private void ShowReceipt()
    {
        Time.timeScale = 0f;
        receiptPanel.SetActive(true);
    }

    public void StartNextDay()
    {
        currentDay++;
        currentTime = 540f;
        isShopClosed = false;
        
        receiptPanel.SetActive(false);
        Time.timeScale = 1f;
        
        customerSpawner.SetActive(true);
        
        UpdateDayText();
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
