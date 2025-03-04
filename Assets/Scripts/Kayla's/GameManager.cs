using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI DayText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    private int currentDay = 1;
    private float currentTime = 540; // 09:00 in minutes (9 * 60)
    private float endTime = 1260; // 21:00 in minutes (21 * 60)
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
        currentTime += Time.deltaTime / timeScale * 15;

        if (currentTime >= endTime)
        {
            currentTime = 540;
            currentDay++;
            UpdateDayText();
        }

        UpdateTimer();
    }

    private void UpdateDayText()
    {
        DayText.text = "Day " + currentDay.ToString();
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
