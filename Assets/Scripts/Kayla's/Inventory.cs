using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>();

    public void AddIngredient(Sprite ingredientSprite)
    {
        foreach (GameObject slot in slots)
        {
            Image slotImage = slot.GetComponentInChildren<Image>();

            if (slotImage.sprite == null)
            {
                slotImage.sprite = ingredientSprite;
                slotImage.enabled = true;
                break;
            }
        }
    }

    public void ClearInventory()
    {
        foreach (GameObject slot in slots)
        {
            Image slotImage = slot.GetComponentInChildren<Image>();
            slotImage.sprite = null;
            slotImage.enabled = false;
        }
    }
}