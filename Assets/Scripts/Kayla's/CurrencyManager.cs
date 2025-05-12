using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public float coins = 100; // Starting coins
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI coinText1;

    private void Update()
    {
        //coinText.text = coins.ToString();
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
