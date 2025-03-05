using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch_room : MonoBehaviour
{
    private Animator moving;
    [SerializeField] private Canvas currCanvas;
    [SerializeField] private Canvas nextCanvas;
    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void switchRoom(Canvas canvas)
    {
        if(nextCanvas != null) currCanvas = nextCanvas;
        nextCanvas = canvas;
        moving.SetTrigger("pindah");
    }
    public void move()
    {
        currCanvas.sortingOrder = 0;
        nextCanvas.sortingOrder = 1;
    }
}
