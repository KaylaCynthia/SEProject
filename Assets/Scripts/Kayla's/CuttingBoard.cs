using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class CuttingBoard : MonoBehaviour
{
    private food food;
    private int count = 0;
    private Image currentIngredient;
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D ingredient)
    {
        if (ingredient == null) return;
        
        food = ingredient.GetComponent<food>();
        if (food == null) return;

        if(food.isCuttable == true && !ingredient.GetComponent<food>().foodName.StartsWith("Diced"))
        {
            count = 0;
            currentIngredient = ingredient.gameObject.GetComponent<Image>();
            // ingredient.gameObject.SetActive(false);
            ingredient.GetComponent<food>().slotPosition = transform.position;
            inventory.RemoveInventory(ingredient.gameObject);
            StartDicing();
        }
    }

    public void StartDicing()
    {
        if(count == 5)
        {
            currentIngredient.sprite = food.dicedSprite;
            currentIngredient.GetComponent<food>().foodName = "Diced " + currentIngredient.GetComponent<food>().foodName;
            currentIngredient.GetComponent<food>().isRefundable = false;
            // currentIngredient.gameObject.SetActive(true);
            inventory.AddIngredient(currentIngredient.gameObject);
        }
        count++;
    }
}