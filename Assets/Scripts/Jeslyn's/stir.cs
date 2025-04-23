using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class stir : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] private bool IsBoil;
    public bool cooked = true;
    //[SerializeField] private GameObject img;
    [SerializeField] private CookingPot pot;
    [SerializeField] private Slider slider;
    public float stir_value = 0f;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 centerScreenPos;
    private Vector2 previousDirection;
    private bool isPointerOver = false;
    public RectTransform inventory;
    Vector3 temp;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        inventory = GameObject.Find("Inventory").GetComponent<RectTransform>();
        temp = inventory.localPosition;

    }

    void Update()
    {
        if (pot.isCooking)
        {
            inventory.localPosition = Vector2.MoveTowards(inventory.localPosition, new Vector3(inventory.localPosition.x, 1000, inventory.localPosition.z), Time.deltaTime * 1000);
        }
        else
        {
            inventory.localPosition = Vector2.MoveTowards(inventory.localPosition, temp, Time.deltaTime * 1000);
        }

        slider.value = stir_value;
        if (stir_value >= 100f)
        {
            cooked = true;
        }
        else if(!isDragging && cooked && !pot.isCooking)
        {
            rectTransform.rotation = Quaternion.identity;
        }
        else if (isDragging)
        {
            Vector2 currentMousePos = Input.mousePosition;
            Vector2 currentDirection = currentMousePos - centerScreenPos;

            if (currentDirection.magnitude > 0.01f) // Prevents NaN or weird values
            {
                float angleDelta = Vector2.SignedAngle(previousDirection, currentDirection);
                rectTransform.Rotate(0f, 0f, angleDelta);
                previousDirection = currentDirection;
            }
        }
    }

/*    public void resetRotation()
    {
        if (!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging && collision.gameObject.name == "stir_inside" && !pot.isCooking && pot.checkIngredientInPot())
        {
            if (!pot.isCooking)
            {
                pot.StartCooking();
                inventory.GetComponent<Animator>().SetBool("up", true);
            }
                if (!IsBoil)
            {
                CancelInvoke("stopCooking");
                Invoke("stopCooking", 1.5f);
            }
        }
        else if (isDragging && collision.gameObject.name == "stir_inside" && pot.isCooking)
        {
            if (!IsBoil)
            {
                CancelInvoke("stopCooking");
                Invoke("stopCooking", 1.5f);
            }
            else
            {
                stir_value += 5f;
            }
        }
    }
    void stopCooking()
    {
        if (pot.isCooking)
        {
            //Debug.Log("gk diaduk");
            pot.StopCooking();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!IsBoil) pointerUp = false;
        if (isPointerOver)
        {
            isDragging = true;
            centerScreenPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
            previousDirection = (Vector2)Input.mousePosition - centerScreenPos;
        }
    }

    public bool pointerUp = false;
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (!IsBoil && pot.isCooking && pot.isReady)
        {
            pointerUp = true;
            CancelInvoke("stopCooking");
            pot.StopCooking();
        }
        else if(!IsBoil && pot.isCooking && !pot.isReady)
        {
            //Debug.Log("cursor keangkat");
            CancelInvoke("stopCooking");
            Invoke("stopCooking", 1f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }
}
