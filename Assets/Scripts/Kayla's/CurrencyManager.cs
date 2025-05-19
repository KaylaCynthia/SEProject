using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public float coins; // Starting coins
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI coinText1;

    //tambahan save coins
    private void Awake()
    {
        if (PlayerPrefs.HasKey("coins") || PlayerPrefs.GetFloat("coins") <= 0)
        {
            coins = 50000;
        }
    }
    //

    private void Update()
    {
        //coinText.text = coins.ToString();
        PlayerPrefs.SetFloat("coins",coins);
        CultureInfo culture = new CultureInfo("id-ID");
        coinText.text = coins.ToString("C", culture);
        coinText1.text = coins.ToString("C", culture);
    }

    public void DecreaseCoins(float amount)
    {
        coins -= amount;
    }

    public void AddCoins(float amount)
    {
        coins += amount;
    }
}
