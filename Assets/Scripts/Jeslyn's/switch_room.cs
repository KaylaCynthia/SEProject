using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch_room : MonoBehaviour
{
    private Animator moving;
    //[SerializeField] private GameObject currency;
    [SerializeField] private GameObject order_notes;
    [SerializeField] private GameObject bento;
    [SerializeField] private Canvas currCanvas;
    [SerializeField] private Canvas nextCanvas;
    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponent<Animator>();
        order_notes.SetActive(false);
        bento.SetActive(false);
        //currency.SetActive(true);
    }

    public void switchRoom(Canvas canvas)
    {
        if(nextCanvas != null) currCanvas = nextCanvas;
        nextCanvas = canvas;
        moving.SetTrigger("pindah");
    }
    public void move()
    {
        if (nextCanvas.gameObject.name == "Canvas_CUSTOMER_ORDER")
        {
            order_notes.SetActive(false);
            bento.SetActive(false);
            //currency.SetActive(true);
        }
        else
        {
            order_notes.SetActive(true);
            bento.SetActive(true);
            //currency.SetActive(false);
        }
        currCanvas.sortingOrder = 0;
        //currCanvas.gameObject.SetActive(false);
        nextCanvas.sortingOrder = 1;
        //nextCanvas.gameObject.SetActive(true);
    }
}
