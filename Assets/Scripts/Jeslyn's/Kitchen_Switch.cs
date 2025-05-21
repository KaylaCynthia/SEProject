using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kitchen_Switch : MonoBehaviour
{
    //[SerializeField] private List<GameObject> cook;
    Vector3 back;
    RectTransform currRoom;
    bool isBento = false;
    [SerializeField] private Button bentoButton;
    TutorialManager tutor;
    // Start is called before the first frame update
    void Start()
    {
        tutor = FindObjectOfType<TutorialManager>();
        currRoom = new RectTransform();
        back = new Vector3(0,-500,0);
/*        foreach (GameObject stir in cook)
        {
            stir.SetActive(false);
        }*/
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
/*            foreach (GameObject stir in cook)
            { 
                if(back == new Vector3(0, -700, 0))
                {
                    stir.SetActive(false);
                }
            }*/
        }
    }
    public void switchRoom(RectTransform room)
    {
        if (currRoom == null && !isFridge)
        {
            if (room.gameObject.name == "cutting board" && tutor.isTutoring && tutor.idx == 10)
            {
                tutor.nextTut();
            }
            else if (room.gameObject.name == "fryingpan" && tutor.isTutoring && tutor.idx == 15)
            {
                tutor.nextTut();
            }
            //currency.SetActive(false);
            currRoom = room;
            back = new Vector3(0, 0, 0);
/*            foreach (GameObject stir in cook)
            {
                if (stir.transform.parent.gameObject == currRoom.gameObject)
                {
                    stir.SetActive(true);
                    break;
                }
            }*/
        }
    }

    //[SerializeField] private GameObject currency;
    public void backToMain(RectTransform room)
    {
        if (currRoom == null)
        {
            if (room.gameObject.name == "cutting board" && tutor.isTutoring && tutor.idx == 12)
            {
                tutor.nextTut();
            }
            //currency.SetActive(true);
            currRoom = room;
            back = new Vector3(0, -700, 0);
        }
    }
    bool isFridge = false;
    public void openFridge(RectTransform fridge)
    {
        if (tutor.isTutoring && tutor.idx == 7 || tutor.idx == 9)
        {
            tutor.nextTut();
        }
        isFridge = !isFridge;
        //currency.SetActive(isFridge);
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
    public void openBento(RectTransform bento)
    {
        if (!isBento && !isFridge)
        {
            isBento = true;
            switchRoom(bento);
        }
        else if(isBento && !isFridge)
        {
            isBento = false;
            backToMain(bento);
        }
    }
    public void hideBento(bool active)
    {
        bentoButton.interactable = !active;
    }
}
