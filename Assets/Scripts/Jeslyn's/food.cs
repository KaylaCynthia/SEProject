using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class food : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    TutorialManager tutor;
    [SerializeField] private float foodPrice;
    public string foodName;
    Inventory inventory;
    CurrencyManager currencyManager;
    DayReceipt dayReceipt;
    public GameObject slot;
    public Vector2 slotPosition = Vector2.zero;
    public bool isParent = false;
    private RectTransform rectTransform;
    public bool isClicked = false;
    public bool isCollideFridge = false;
    public bool released = false;
    public bool isCuttable = false;
    public bool isRefundable = true;
    public Sprite eggSprite;
    public Sprite normalSprite;
    public Sprite dicingSprite;
    public Sprite dicedPartSprite;
    public Sprite dicedSprite;
    public bool isCooking;
    // Start is called before the first frame update
    void Start()
    {
        tutor = FindObjectOfType<TutorialManager>();
        normalSprite = GetComponent<Image>().sprite;
        currencyManager = FindObjectOfType<CurrencyManager>();
        inventory = FindObjectOfType<Inventory>();
        dayReceipt = FindObjectOfType<DayReceipt>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slot != null && !isCooking)
        {
            slotPosition = slot.transform.position;
        }
        if (!isCooking && !isClicked && !isParent)
        {
            //rectTransform.position = Vector2.MoveTowards(rectTransform.position, slotPosition, Time.deltaTime*1000);
            rectTransform.position = slotPosition;
        }
        else if (isCooking)
        {
            GameObject cut = GameObject.Find("cut");
            rectTransform.position = new Vector3(cut.transform.position.x, cut.transform.position.y - 50f, cut.transform.position.z);
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
        if (!isCooking && collision.name == "Inventory" && !inventory.isFull && !inventory.AlreadyInInventory(gameObject))
        {
            inventory.AddIngredient(gameObject);
        }
        if (collision.name == "Fridge" && inventory.AlreadyInInventory(gameObject) && isRefundable)
        {
            isCollideFridge = true;
        }
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
            clone.transform.localScale = Vector3.one * 0.5f;
            clone.GetComponent<food>().isParent = false;
            clone.GetComponent<food>().isClicked = true;
            // clone.GetComponent<RectTransform>().localScale = Vector3.one * 0.4f;
            Vector2 size = GameObject.Find("Slot1").transform.GetChild(0).GetComponent<RectTransform>().rect.size;
            clone.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            clone.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            clone.GetComponent<RectTransform>().localScale = Vector3.one;
            // clone = Instantiate(gameObject, inventory.getSlotPosition(clone));
        }
        if (!isParent && inventory.isDeleteMode == true && inventory.AlreadyInInventory(gameObject))
        {
            inventory.RemoveInventory(gameObject);
            Destroy(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isParent && !isCooking)
        {
            transform.position = eventData.position;
        }
        else if(isParent)
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
            if (tutor.isTutoring && tutor.idx == 8)
            {
                tutor.nextTut();
            }
            clone.GetComponent<food>().isClicked = false;
            clone.GetComponent<food>().released = true;
            currencyManager.DecreaseCoins(foodPrice);
            dayReceipt.iExpenses += foodPrice;
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
