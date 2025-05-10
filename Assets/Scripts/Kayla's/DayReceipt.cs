using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DayReceipt : MonoBehaviour
{
    public float totalEarnings;
    public float iExpenses;
    public float netEarnings;
    public float customerServed;
    public List<float> satisfactionRate;

    [SerializeField] private List<TextMeshProUGUI> displayText;

    public void UpdateReceiptData()
    {
        if (displayText.Count >= 5)
        {
            displayText[0].text = totalEarnings.ToString();
            displayText[1].text = iExpenses.ToString();
            displayText[2].text = netEarnings.ToString();
            displayText[3].text = customerServed.ToString();
            
            if (satisfactionRate.Count > 0)
            {
                float averageSatisfaction = CalculateAverageSatisfaction();
                displayText[4].text = averageSatisfaction.ToString("F1");
            }
            else
            {
                displayText[4].text = "N/A";
            }
        }
    }

    private float CalculateAverageSatisfaction()
    {
        int total = 0;
        foreach (int rating in satisfactionRate)
        {
            total += rating;
        }
        return (float)total / satisfactionRate.Count;
    }

    public void UpdateSatisfactionRate(float sr)
    {
        satisfactionRate.Add(sr);   
    }

    public void ResetSatisfactionRate()
    {
        satisfactionRate.Clear();
    }
}