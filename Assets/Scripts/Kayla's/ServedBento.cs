using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ServedBento : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Customer targetCustomer;
    private bool isDragging = false;
    private Vector2 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        targetCustomer = FindObjectOfType<Customer>();
        isDragging = true;
        // originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDragging && targetCustomer != null && targetCustomer.tag == "Customer")
        {
            targetCustomer.SubmitOrder(this.gameObject.name);
            Destroy(gameObject);
        }
        // else
        // {
        //     transform.position = originalPosition;
        // }
    }
}

