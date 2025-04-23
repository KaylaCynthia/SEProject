using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class CuttingBoard : MonoBehaviour
{
    //tambahan
    List<GameObject> parts = new List<GameObject>();
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
            food.GetComponent<Image>().raycastTarget = false;
            food.isCooking = true;
            food.transform.localScale = Vector3.one*5f;
            //
            back.interactable = false;
            GetComponent<BoxCollider2D>().enabled = false;
            count = 0;
            currentIngredient = ingredient.gameObject.GetComponent<Image>();
            // ingredient.gameObject.SetActive(false);
            ingredient.GetComponent<food>().slotPosition = new Vector3(transform.position.x, transform.position.y-50f,transform.position.z);
            inventory.RemoveInventory(ingredient.gameObject);
            //tambahan
            //StartDicing();
            transform.GetChild(0).gameObject.SetActive(true);
            //
        }            
    }

    public void StartDicing()
    {
        if(count == 4)
        {
            //tambahan
            foreach (GameObject part in parts)
            {
                Destroy(part);
            }
            parts.Clear();
            currentIngredient.fillAmount = 1f;
            food.transform.localScale = Vector3.one;
            isCutting = false;
            transform.GetChild(0).gameObject.SetActive(false);
            food.isCooking = false;
            food.slotPosition = food.slot.transform.position;
            food.GetComponent<Image>().raycastTarget = true;
            //

            back.interactable = true;
            GetComponent<BoxCollider2D>().enabled = true;
            currentIngredient.sprite = food.dicedSprite;
            food.GetComponent<RectTransform>().position = food.slotPosition;
            food.foodName = "Diced " + currentIngredient.GetComponent<food>().foodName;
            food.isRefundable = false;
            // currentIngredient.gameObject.SetActive(true);
            inventory.AddIngredient(currentIngredient.gameObject);
        }

        //tambahan
        else
        {
            GameObject dice = new GameObject("dicePart");
            dice.AddComponent<dicedPart>().target = new Vector2(food.transform.localPosition.x + food.GetComponent<RectTransform>().rect.width  - count*10, transform.localPosition.y);
            dice.AddComponent<RectTransform>().localScale = Vector3.one * 0.7f;
            dice.AddComponent<Image>().sprite = food.dicedPartSprite;
            dice.GetComponent<Image>().raycastTarget = false;
            parts.Add(Instantiate(dice, food.transform));
            currentIngredient.fillAmount -= 0.2f;
        }
        count++;
    }
}