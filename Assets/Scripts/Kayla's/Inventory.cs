using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>();
    [SerializeField] private List<GameObject> inventory = new List<GameObject>();
    public bool isFull = false;
    [SerializeField] private int idx = 0;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            slots.Add(transform.GetChild(i).gameObject);
            inventory.Add(null);
        }
    }
    public void AddIngredient(GameObject ingredientSprite)
    {
        Image slotImage = slots[idx].transform.GetChild(0).GetComponent<Image>();
        slotImage.sprite = ingredientSprite.GetComponent<Image>().sprite;
        slotImage.enabled = true;
        inventory[idx] = ingredientSprite;
        getIndex();

        //biar gk perlu loop, kt pake index aja (gk gt keliatan sih pengaruhnya, cmn ini biar gk gt bnyk loop)
        //biar gk gt ngeberatin device/memory :3
/*        foreach (GameObject slot in slots)
        {
            Image slotImage = slot.GetComponentInChildren<Image>();
            if (slotImage.sprite == null)
            {
                slotImage.sprite = ingredientSprite.GetComponent<Image>().sprite;
                slotImage.enabled = true;
                break;
            }
        }*/
    }

    public void ClearInventory()
    {
        foreach (GameObject slot in slots)
        {
            Image slotImage = slot.GetComponentInChildren<Image>();
            slotImage.sprite = null;
            slotImage.enabled = false;
        }
        idx = 0;
    }

    //tambahan dr Jes, buat trash
    public void RemoveInventory(GameObject food)
    {
        foreach (GameObject slot in inventory)
        {
            if (slot == food)
            {
                //for some reason ini error pas ku cb T_T, jd ku ganti
                //Image slotImage = slot.GetComponentInChildren<Image>();
                Image slotImage = slots[inventory.IndexOf(food)].transform.GetChild(0).GetComponent<Image>();
                inventory[inventory.IndexOf(food)] = null;
                slotImage.sprite = null;
                slotImage.enabled = false;
                getIndex();
                return;
            }
        }
        getIndex();
    }
    public bool inInventory(GameObject food)
    {
        foreach (GameObject slot in inventory)
        {
            if (slot == food)
            {
                return true;
            }
        }
        return false;
    }
    void getIndex()
    {
        for (int i=0;i<inventory.Count;i++)
        {
            if (inventory[i] == null)
            {
                idx = i;
                return;
            }
        }
        isFull = true;
        idx = slots.Count - 1;
    }
}