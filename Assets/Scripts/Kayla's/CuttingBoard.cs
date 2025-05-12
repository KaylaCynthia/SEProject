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
    [SerializeField] private GameObject pisau;
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
    //tambahan
    private void Update()
    {
        if(isCutting)
        {

            pisau.GetComponent<Image>().color = Color.Lerp(pisau.GetComponent<Image>().color, Color.white, Time.deltaTime*10f);
        }
        else
        {
            pisau.GetComponent<Image>().color = Color.Lerp(pisau.GetComponent<Image>().color, Color.clear, Time.deltaTime*5f);
        }
    }

    //
    private void OnTriggerEnter2D(Collider2D ingredient)
    {
        if (ingredient == null) return;
        food = ingredient.GetComponent<food>();
        if (food == null) return;

        if(!isCutting && food.isCuttable == true && !ingredient.GetComponent<food>().foodName.StartsWith("Diced"))
        {
            //modif
            isCutting = true;
            food.isCooking = true;
            inventory.RemoveInventory(ingredient.gameObject);
            food.slotPosition = new Vector3(transform.position.x, transform.position.y-50f,transform.position.z);
            food.GetComponent<RectTransform>().position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            food.GetComponent<Image>().sprite = food.dicingSprite;
            food.GetComponent<Image>().raycastTarget = false;
            food.transform.localScale = food.transform.localScale * 5f;
            //
            back.interactable = false;
            GetComponent<BoxCollider2D>().enabled = false;
            count = 0;
            currentIngredient = ingredient.gameObject.GetComponent<Image>();
            pisau.transform.localPosition = new Vector2(food.transform.localPosition.x + food.GetComponent<RectTransform>().rect.width*2.5f, transform.localPosition.y);
            // ingredient.gameObject.SetActive(false);
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
/*            pisau.transform.parent = GameObject.Find("Canvas_ALL").transform;
            pisau.transform.SetSiblingIndex(7);*/
            pisau.GetComponent<Animator>().Play("cut");
            //GameObject[] unwantedParts = GameObject.FindGameObjectsWithTag("Dice");
            foreach (GameObject part in parts)
            {
                part.transform.parent = GameObject.Find("FoodInventory").transform;
                //Destroy(part);
                part.GetComponent<dicedPart>().destroying = true;
            }
            parts.Clear();
            currentIngredient.fillAmount = 1f;
            food.transform.localScale = food.transform.localScale / 5f;
            isCutting = false;
            transform.GetChild(0).gameObject.SetActive(false);
            back.interactable = true;

            GetComponent<BoxCollider2D>().enabled = true;
            currentIngredient.sprite = food.dicedSprite;
            food.foodName = "Diced " + food.foodName;
            food.isRefundable = false;

            inventory.AddIngredient(currentIngredient.gameObject);
            food.isCooking = false;
            food.GetComponent<Image>().raycastTarget = true;
            //food.slotPosition = food.slot.transform.position;
            //food.GetComponent<RectTransform>().position = food.slotPosition;
            //

            // currentIngredient.gameObject.SetActive(true);
        }

        //tambahan
        else
        {
            pisau.GetComponent<Animator>().StopPlayback();
            pisau.GetComponent<Animator>().Play("cut");
            GameObject dice = new GameObject("dicePart");
            dice.tag = "Dice";
            parts.Add(dice);
            dice.transform.parent = food.transform;
            dice.transform.position = pisau.transform.position;
            dice.AddComponent<dicedPart>().target = new Vector2(food.transform.localPosition.x + (food.GetComponent<RectTransform>().rect.width - 31) - count*10f, transform.localPosition.y);
            dice.AddComponent<RectTransform>().localScale = Vector3.one * 0.3f;
            dice.AddComponent<Image>().color = Color.clear;
            dice.GetComponent<Image>().sprite = food.dicedPartSprite;
            dice.GetComponent<Image>().raycastTarget = false;
            //parts.Add(Instantiate(dice, food.transform));
            currentIngredient.fillAmount -= 0.2f;
        }
        count++;
        pisau.transform.localPosition = new Vector2((food.transform.localPosition.x + food.GetComponent<RectTransform>().rect.width*2.5f) - count * 60f, transform.localPosition.y);
    }
}