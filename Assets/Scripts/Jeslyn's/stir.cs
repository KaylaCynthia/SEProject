using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class stir : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool cooked = false;
    //[SerializeField] private GameObject img;
    [SerializeField] private CookingPot pot;
    [SerializeField] private Slider slider;
    public float stir_value = 0f;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 centerScreenPos;
    private Vector2 previousDirection;
    private bool isPointerOver = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        slider.value = stir_value;
        if (stir_value >= 100f)
        {
            cooked = true;
        }
        if (isDragging && !cooked)
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
        else if(cooked)
        {
            rectTransform.Rotate(0f, 0f, 0f);
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
        if (isDragging && collision.gameObject.name == "stir_inside")
        {
            stir_value += 10f;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPointerOver && !cooked)
        {
            isDragging = true;
            centerScreenPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
            previousDirection = (Vector2)Input.mousePosition - centerScreenPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
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
