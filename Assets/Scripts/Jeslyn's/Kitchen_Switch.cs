using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen_Switch : MonoBehaviour
{
    Vector3 back;
    RectTransform currRoom;
    // Start is called before the first frame update
    void Start()
    {
        currRoom = new RectTransform();
        back = new Vector3(0,-500,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currRoom != null)
        {
            currRoom.localPosition = Vector2.MoveTowards(currRoom.localPosition, back, Time.deltaTime * 3000);
        }
        if (currRoom != null && Vector2.Distance(currRoom.localPosition, back) < 0.001f)
        {
            currRoom = null;
        }
    }
    public void switchRoom(RectTransform room)
    {
        if (currRoom == null && !isFridge)
        {
            currency.SetActive(false);
            currRoom = room;
            back = new Vector3(0, 0, 0);
        }
    }

    [SerializeField] private GameObject currency;
    public void backToMain(RectTransform room)
    {
        if (currRoom == null)
        {
            currency.SetActive(true);
            currRoom = room;
            back = new Vector3(0, -700, 0);
        }
    }
    bool isFridge = false;
    public void openFridge(RectTransform fridge)
    {
        isFridge = !isFridge;
        currency.SetActive(isFridge);
        if (currRoom == null)
        {
            if (isFridge)
            {
                currRoom = fridge;
                back = new Vector3(1000, 0, 0);
            }
            else
            {
                currRoom = fridge;
                back = new Vector3(0, 0, 0);
            }
        }
    }
}
