using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class food : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private float foodPrice;
    Inventory inventory;
    CurrencyManager currencyManager;
    public Vector2 slotPosition = Vector2.zero;
    public bool isParent = false;
    private RectTransform rectTransform;
    public bool isClicked = false;
    public bool isCollideFridge = false;
    public bool released = false;
    // Start is called before the first frame update
    void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        inventory = FindObjectOfType<Inventory>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClicked && !isParent)
        {
            //rectTransform.position = Vector2.MoveTowards(rectTransform.position, slotPosition, Time.deltaTime*1000);
            rectTransform.position = slotPosition;
        }
        if (!isClicked && isCollideFridge)
        {
            inventory.RemoveInventory(gameObject);
            Destroy(gameObject);
            currencyManager.AddCoins(foodPrice);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.name == "TRASH" && isClicked)
        // {
        //     inventory.RemoveInventory(gameObject);
        //     Destroy(gameObject);
        // }
        if (collision.name == "Inventory" && !inventory.isFull && !inventory.AlreadyInInventory(gameObject))
        {
            inventory.AddIngredient(gameObject);
        }
        if (collision.name == "Fridge" && inventory.AlreadyInInventory(gameObject))
        {
            isCollideFridge = true;
        }
        //buat nanti :)
/*        if (collision.name == "Blend")
        {

        }
        else if (collision.name == "Cut")
        {

        }
        else if (collision.name == "Cook")
        {

        }*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Inventory" && !released && inventory.AlreadyInInventory(gameObject))
        {
            inventory.RemoveInventory(gameObject);
            slotPosition = Vector2.zero;
        }
        if (collision.name == "Fridge" && inventory.AlreadyInInventory(gameObject))
        {
            isCollideFridge = false;
        }
    }

    GameObject clone;
    public void OnBeginDrag(PointerEventData eventData)
    {
        isClicked = true;
        if (isParent)
        {
            clone = Instantiate(gameObject,GameObject.Find("FoodInventory").transform);
            clone.GetComponent<food>().isParent = false;
            clone.GetComponent<food>().isClicked = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isParent)
        {
            transform.position = eventData.position;
        }
        else
        {
            clone.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isClicked = false;
        if (!isParent && slotPosition == Vector2.zero)
        {
            Destroy(gameObject);
        }
        else if (isParent && clone.GetComponent<food>().slotPosition == Vector2.zero)
        {
            Destroy(clone);
        }
        else if(isParent && clone.GetComponent<food>().slotPosition != Vector2.zero)
        {
            clone.GetComponent<food>().isClicked = false;
            clone.GetComponent<food>().released = true;
            currencyManager.DecreaseCoins(foodPrice);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(inventory.isDeleteMode == true && inventory.AlreadyInInventory(gameObject))
        {
            inventory.RemoveInventory(gameObject);
            Destroy(gameObject);
        }
    }
}
