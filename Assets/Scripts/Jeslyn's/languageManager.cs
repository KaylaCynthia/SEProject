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
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "Indonesia")
        {
            text.text = indonesia;
        }
        else if(PlayerPrefs.GetString("language") == "English")
        {
            text.text = english;
        }
    }
}
