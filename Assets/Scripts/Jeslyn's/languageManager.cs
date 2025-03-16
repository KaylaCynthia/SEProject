using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public class languageManager : MonoBehaviour
{
    [SerializeField] [TextArea] private string indonesia;
    [SerializeField] [TextArea] private string english;
    private TextMeshProUGUI text;
    [SerializeField] private bool onlyInStart = false;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (PlayerPrefs.GetString("language") == "Indonesia")
        {
            text.text = indonesia;
        }
        else if (!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "English")
        {
            text.text = english;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!onlyInStart)
        {
            if (PlayerPrefs.GetString("language") == "Indonesia")
            {
                text.text = indonesia;
            }
            else if(!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "English")
            {
                text.text = english;
            }
        }
    }
}
