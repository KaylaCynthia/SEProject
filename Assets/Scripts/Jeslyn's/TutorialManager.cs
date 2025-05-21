using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    [TextArea] private List<string> tutorialIND;
    [SerializeField]
    [TextArea] private List<string> tutorialENG;
    [SerializeField] private List<GameObject> imgTarget;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator animFork;
    [SerializeField] private TextMeshProUGUI text;
    int tempLength = 0;
/*    [Serializable]
    struct unInteractableButtons
    {
        public List<Button> buttons;
        //public bool pointing;
    }
    [SerializeField] private List<unInteractableButtons> unInteractable;
    [SerializeField] private unInteractableButtons curr_unInteractable;*/
    public bool isTutoring;
    CustomerSpawner spawner;
    public int idx = -1;
    bool isTalking = false;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<CustomerSpawner>();
        if (PlayerPrefs.GetInt("day") > 1)
        {
            isTutoring = false;
            StartCoroutine(spawner.SpawnCustomer());
            anim.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            isTutoring = true;
            idx = -1;
            nextTut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (idx == 2 && imgTarget[3] == null)
        {
            Customer cust = null;
            while (cust == null)
            {
                cust = FindObjectOfType<Customer>();
            }
            //imgTarget[3] = cust.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
            imgTarget[4] = cust.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        }
        //idx yg bs klik asal = 0, 1, 2, 3, 5, 13, 14, 15, 20, 21
        if (idx == 0 ||
            idx == 1 ||
            idx == 2 ||
            idx == 3 ||
            idx == 5 ||
            idx == 13 ||
            idx == 14 ||
            idx == 20 ||
            idx == 21 )
        {
            if (Input.anyKeyDown && text.maxVisibleCharacters == tempLength)
            {
                nextTut();
            }
        }
        animFork.SetBool("talk",isTalking);
        if (imgTarget[idx] != null)
        {
            // 1. Ambil posisi screen dari source
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(GameObject.Find("Canvas_ALL").GetComponent<Canvas>().worldCamera, imgTarget[idx].GetComponent<RectTransform>().position);
            // 2. Konversi screen pos ke posisi lokal di canvas target
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                animFork.GetComponent<RectTransform>().parent.parent as RectTransform, // parent UI dari target
                screenPos,
                animFork.transform.parent.parent.parent.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : animFork.transform.parent.parent.GetComponent<Canvas>().worldCamera,
                out Vector2 localPoint
            );
            // 3. Terapkan ke target
            animFork.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(animFork.GetComponent<RectTransform>().localPosition, localPoint, Time.deltaTime * 500f);
            //animFork.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(animFork.GetComponent<RectTransform>().localPosition, imgTarget[idx].GetComponent<RectTransform>().anchoredPosition, Time.deltaTime * 100f);
            animFork.SetBool("points",true);
        }
        else
        {
            animFork.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(animFork.GetComponent<RectTransform>().localPosition, new Vector3(170, 0, 0), Time.deltaTime * 500f);
            animFork.SetBool("points",false);
        }
    }
    IEnumerator talking()
    {
        while (true)
        {
            if (PlayerPrefs.GetString("language") == "Indonesia")
            {
                tempLength = tutorialIND[idx].Length;
            }
            else if (!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "English")
            {
                tempLength = tutorialENG[idx].Length;
            }
            if (text.maxVisibleCharacters < tempLength)
            {
                text.maxVisibleCharacters++;
            }
            else
            {
                isTalking = false;
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
    public void nextTut()
    {
        if (idx < 21)
        {
            idx++;
            if (idx == 2)
            {
                StartCoroutine(spawner.SpawnCustomer());
            }
            else if(idx == 10)
            {
                order_notes note = FindObjectOfType<order_notes>();
                note.open_note();
            }
            //index next = 2 - 4, 6 - 20
            if (idx == 2 || idx == 3 || idx == 4 || idx == 5 || idx == 6 || idx == 7 || idx == 8 || idx == 9 || idx == 10 ||
                idx == 11 || idx == 12 || idx == 13 || idx == 14 || idx == 15 || idx == 16 || idx == 17 ||
                 idx == 18 || idx == 19 || idx == 20)
            {
                anim.SetTrigger("next");
            }
            //anim.SetTrigger("next");
            isTalking = true;
            text.maxVisibleCharacters = 0;
            if (PlayerPrefs.GetString("language") == "Indonesia")
            {
                text.text = tutorialIND[idx];
            }
            else if (!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "English")
            {
                text.text = tutorialENG[idx];
            }
            StartCoroutine(talking());
            //curr_unInteractable = unInteractable[idx];
        }
        else
        {
            isTutoring = false;
            anim.SetTrigger("next");
            GameManager gm = FindObjectOfType<GameManager>();
            gm.timerStart = true;
        }
    }
}
