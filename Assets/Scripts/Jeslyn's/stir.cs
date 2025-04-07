using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class stir : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    //[SerializeField] private GameObject img;
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
        if (isDragging)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDragging && collision.gameObject.name == "stir_inside")
        {
            stir_value += 1f;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPointerOver)
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
