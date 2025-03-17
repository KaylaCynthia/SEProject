using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>();
    public bool isFull = false;
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
            //tambahan, idx itu buat nentuin posisi mana yg kosong di slot
            int idx = 0;
            if (slot == null)
            {
                idx = slots.IndexOf(slot);
                slots[idx] = ingredientSprite;

                //ini ku comment gegara gk ngaruh, for some reason dia itu sama kek getcomponent biasa T_T
                //Image slotImage = slot.GetComponentInChildren<Image>();

                Image slotImage = transform.GetChild(idx).GetChild(0).GetComponent<Image>();
                ingredientSprite.GetComponent<food>().slotPosition = slotImage.rectTransform.position;
                if (slotImage.sprite == null)
                {
                    slotImage.sprite = ingredientSprite.GetComponent<Image>().sprite;
                    slotImage.enabled = true;

                    //kl uda msk di posisi terakhir, berarti inventorynya full
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
                int idx = slots.IndexOf(slot);
                //Image slotImage = slot.GetComponentInChildren<Image>();
                Image slotImage = transform.GetChild(idx).GetChild(0).GetComponent<Image>();
                slotImage.sprite = null;
                slotImage.enabled = false;
                idx = slots.IndexOf(slot);
                slots[idx] = null;
                return;
            }
        }
    }
    //buat ngecek kl foodnya udh ada di inventory ato blm
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
}