using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class order_notes : MonoBehaviour
{
    RectTransform RectTransform;
    [SerializeField] private Vector2 currPos;
    //[SerializeField] private RectTransform arrow;
    [SerializeField] private Vector3 currRot;
    [SerializeField] private Vector3 nextRot;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float progress = 0;
    [SerializeField] private List<TextMeshProUGUI> ingredients;
    [SerializeField] private List<TextMeshProUGUI> steps;
    [SerializeField] private List<TextMeshProUGUI> tools;
    [SerializeField] private List<GameObject> checklists;
    TutorialManager tutor;
    // Start is called before the first frame update
    void Start()
    {
        tutor = FindObjectOfType<TutorialManager>();
        RectTransform = GetComponent<RectTransform>();
        currPos = RectTransform.localPosition;
        //currPos.x = -currPos.x;
        currRot = nextRot = new Vector3(0,0,-90);
        foreach (TextMeshProUGUI m in ingredients)
        {
            m.text = "- ";
        }
        foreach (TextMeshProUGUI m in steps)
        {
            m.text = " ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = Mathf.MoveTowards(progressBar.value, progress, Time.deltaTime*100);
        currPos.y = RectTransform.localPosition.y;
        RectTransform.localPosition = Vector2.MoveTowards(RectTransform.localPosition, currPos, Time.deltaTime*1000);
        currRot = Vector3.MoveTowards(currRot, nextRot, Time.deltaTime*500);
        //arrow.rotation = Quaternion.Euler(currRot);
    }

    bool isOpen = false;
    public void open_note()
    {
        if (tutor.isTutoring && tutor.idx == 6)
        {
            tutor.nextTut();
        }
        isOpen = !isOpen;
        if (isOpen)
        {
            currPos.x -= 400;
            nextRot.z = 90;
        }
        else
        {
            currPos.x += 400;
            nextRot.z = -90;
        }
    }

    public void SetOrder(List<string> step, List<string> ingredient, List<string> tool)
    {
        progress = 0;
        int idx1 = 0;
        int idx2 = 0;
        //int idx3 = 0;
        foreach (TextMeshProUGUI m in ingredients)
        {
            if (idx1 < ingredient.Count)
            {
                m.text = "- ";
                m.text += ingredient[idx1];
                idx1++;
            }
            else
            {
                break;
            }
        }
        foreach (TextMeshProUGUI m in steps)
        {
            if (idx2 < step.Count)
            {
                m.text = step[idx2];
                idx2++;
            }
            else
            {
                return;
            }
        }

        //will be used later 
/*        foreach (TextMeshProUGUI m in tools)
        {
            m.text = tool[idx3];
            idx3++;
        }*/
    }
    public void check(int idx)
    {
        checklists[idx].SetActive(true);
    }
    public void uncheck(int idx)
    {
        checklists[idx].SetActive(true);
    }
    public void resetChecklist()
    {
        foreach (GameObject check in checklists)
        {
            check.SetActive(false);
        }
    }
}
