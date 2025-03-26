using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>();
    public bool isFull = false;
    public bool isDeleteMode = false;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            slots.Add(null);
        }
    }
    public void AddIngredient(GameObject ingredientSprite)
    {
        foreach (GameObject slot in slots)
        {
            int idx = 0;
            if (slot == null)
            {
                idx = slots.IndexOf(slot);
                slots[idx] = ingredientSprite;

                //Image slotImage = slot.GetComponentInChildren<Image>();

                Image slotImage = transform.GetChild(idx).GetChild(0).GetComponent<Image>();
                ingredientSprite.GetComponent<food>().slotPosition = slotImage.rectTransform.position;
                if (slotImage.sprite == null)
                {
                    slotImage.sprite = ingredientSprite.GetComponent<Image>().sprite;
                    slotImage.enabled = true;

                    if (idx == slots.Count - 1)
                    {
                        isFull = true;
                    }
                    return;
                }
            }
        }
    }

    public void ClearInventory()
    {
        foreach (GameObject slot in slots)
        {
            //Image slotImage = slot.GetComponentInChildren<Image>();
            Image slotImage = transform.GetChild(slots.IndexOf(slot)).GetChild(0).GetComponent<Image>();
            slotImage.sprite = null;
            slotImage.enabled = false;
        }
    }

    //tambahan dr Jes, buat trash
    public void RemoveInventory(GameObject food)
    {
        foreach (GameObject slot in slots)
        {
            if (slot == food)
            {
                isFull = false;
                int idx = slots.IndexOf(slot);
                Image slotImage = transform.GetChild(idx).GetChild(0).GetComponent<Image>();
                slotImage.sprite = null;
                slotImage.enabled = false;
                idx = slots.IndexOf(slot);
                slots[idx] = null;
                return;
            }
        }
    }

    public bool AlreadyInInventory(GameObject food)
    {
        foreach (GameObject slot in slots)
        {
            if (slot == food)
            {
                return true;
            }
        }
        return false;
    }

    public void OnDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
    }
}