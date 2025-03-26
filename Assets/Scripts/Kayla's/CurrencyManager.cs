using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public float coins = 100; // Starting coins
    [SerializeField] private TextMeshProUGUI coinText;

    private void Update()
    {
        coinText.text = coins.ToString();
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
