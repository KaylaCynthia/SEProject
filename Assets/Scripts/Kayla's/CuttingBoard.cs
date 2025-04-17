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
    //tambahan
    [SerializeField] private Button back;
    bool isCutting = false;
    //

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

        if(!isCutting && food.isCuttable == true && !ingredient.GetComponent<food>().foodName.StartsWith("Diced"))
        {
            isCutting = true;
            //modif
            food.isCooking = true;
            //
            back.interactable = false;
            GetComponent<BoxCollider2D>().enabled = false;
            count = 0;
            currentIngredient = ingredient.gameObject.GetComponent<Image>();
            // ingredient.gameObject.SetActive(false);
            ingredient.GetComponent<food>().slotPosition = transform.position;
            inventory.RemoveInventory(ingredient.gameObject);
            StartDicing();
            //tambahan
            transform.GetChild(0).gameObject.SetActive(true);
            //
        }            
    }

    public void StartDicing()
    {
        if(count == 5)
        {
            //tambahan
            isCutting = false;
            transform.GetChild(0).gameObject.SetActive(false);
            //

            back.interactable = true;
            GetComponent<BoxCollider2D>().enabled = true;
            currentIngredient.sprite = food.dicedSprite;
            food.isCooking = false;
            food.foodName = "Diced " + currentIngredient.GetComponent<food>().foodName;
            food.isRefundable = false;
            // currentIngredient.gameObject.SetActive(true);
            inventory.AddIngredient(currentIngredient.gameObject);
        }
        count++;
    }
}