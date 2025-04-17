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
        //tambahan krn trash msk inventory
        slots[0] = transform.GetChild(0).gameObject;
        tutupanDeleteMode.SetActive(false);
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
                //tambahan
                //ingredientSprite.transform.parent = slotImage.transform;
                ingredientSprite.GetComponent<food>().slot = slotImage.gameObject;
                ingredientSprite.GetComponent<food>().slotPosition = slotImage.rectTransform.position;

                if (slotImage.sprite == null)
                {
                    slotImage.sprite = ingredientSprite.GetComponent<Image>().sprite;
                    slotImage.enabled = true;

                    //modif -> logicnya salah ternyata
                    /*if (idx == slots.Count - 1)
                    {
                        isFull = true;
                    }
                    return;
                    */
                    for (int i = 0; i < slots.Count; i++)
                    {
                        if (slots[i] == null)
                        {
                            isFull = false;
                            break;
                        }
                        else if (i == slots.Count - 1 && slots[i] != null)
                        {
                            isFull = true;
                        }
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
        //tambahan krn trash msk inventory
        slots[0] = transform.GetChild(0).gameObject;
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

    [SerializeField] private GameObject tutupanDeleteMode;
    public void OnDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
        tutupanDeleteMode.SetActive(isDeleteMode);

    }
}