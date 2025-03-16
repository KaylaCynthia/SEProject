using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class food : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "TRASH")
        {
            inventory.RemoveInventory(gameObject);
            Destroy(gameObject);
        }
        else if (collision.name == "Inventory" && !inventory.isFull && !inventory.inInventory(gameObject))
        {
            inventory.AddIngredient(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
