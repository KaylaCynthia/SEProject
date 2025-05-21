using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Globalization;

public class Customer : MonoBehaviour
{
    //tambahan
    [SerializeField] private List<Color> clothe;
    [SerializeField] private List<Color> hair;
    [SerializeField] private List<Image> hair_sprite;
    [SerializeField] private List<Image> clothe_sprite;
    [SerializeField] private List<GameObject> eyes;
    [SerializeField] private List<GameObject> clothes;
    [SerializeField] private List<GameObject> hair_front;
    [SerializeField] private List<GameObject> hair_back;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator mainAnim;
    order_notes notes;
    TutorialManager tutor;
    //
    private TextMeshProUGUI patienceText;
    public float patience = 100f;
    public List<string> dialogues;
    public string orderName;
    public List<string> ingredients;
    public float price;
    private float tips;
    public string currentOrder;
    private TextMeshProUGUI dialogueText;
    [SerializeField] private Button OKButton;
    [SerializeField] private Button WHATButton;
    [SerializeField] private Button WHATButton1;
    private GameObject dialogueBubble;
    private int currentIdx;
    private bool isWaitingForOrder = false;

    public delegate void OrderCompleted();
    public event OrderCompleted OnOrderCompleted;
    private CurrencyManager currencyManager;
    private DayReceipt dayReceipt;
    
    //tambahan
    private void Awake()
    {
        tutor = FindObjectOfType<TutorialManager>();
        mainAnim = GetComponent<Animator>();
        notes = FindObjectOfType<order_notes>();
        int rand_hair_color = Random.Range(0, hair.Count);
        int rand_clothe_color = Random.Range(0, clothe.Count);
        foreach (Image obj in hair_sprite)
        {
            obj.color = hair[rand_hair_color];
        }
        foreach (Image obj in clothe_sprite)
        {
            obj.color = clothe[rand_clothe_color];
        }

        int rand_eyes = Random.Range(0, eyes.Count);
        int rand_clothe = Random.Range(0, clothes.Count);
        int rand_hairB= Random.Range(0, hair_back.Count);
        int rand_hairF = Random.Range(0, hair_front.Count);
        for(int i=0;i<eyes.Count;i++)
        {
            if (i != rand_eyes)
            {
                eyes[i].SetActive(false);
            }
        }
        for (int i = 0; i < clothes.Count; i++)
        {
            if (i != rand_clothe)
            {
                clothes[i].SetActive(false);
            }
        }
        for (int i = 0; i < hair_back.Count; i++)
        {
            if (i != rand_hairB)
            {
                hair_back[i].SetActive(false);
            }
        }
        for (int i = 0; i < hair_front.Count; i++)
        {
            if (i != rand_hairF)
            {
                hair_front[i].SetActive(false);
            }
        }
    }
    //

    //modif
/*    private void Start()
    {
        StartDialogue();
        patienceText = GameObject.Find("notes").transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        dayReceipt = FindObjectOfType<DayReceipt>();
        dialogueBubble = transform.Find("DialogueBubble").gameObject;
        dialogueText = dialogueBubble.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        StartCoroutine(patienceDrop());
        SetCurrentOrder();
    }*/
    public void startOrdering()
    {
        mainAnim.Play("in");
        patienceText = GameObject.Find("notes").transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        dayReceipt = FindObjectOfType<DayReceipt>();
        dialogueBubble = transform.Find("DialogueBubble").gameObject;
        dialogueText = dialogueBubble.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        StartDialogue();
        StartCoroutine(patienceDrop());
        SetCurrentOrder();
    }
    //

    Coroutine animate;
    bool isLeaving = false;
    
    private void Update()
    {
        if(patienceText != null) patienceText.text = patience.ToString() + "%";
        if (!isLeaving && currentIdx == dialogues.Count - 2 && dialogueText.maxVisibleCharacters == dialogues[currentIdx].Length)
        {
            isLeaving = true;
            CustomerLeaves();
        }
    }

    public void StartDialogue()
    {
        //dialogueBubble.SetActive(true);
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = dialogues[currentIdx];
        animate = StartCoroutine(animationText());
        isWaitingForOrder = true;
        OKButton.interactable = true;
        WHATButton.interactable = true;
        // StartCoroutine(CloseDialogue());
    }

    IEnumerator animationText()
    {
        while (true)
        {
            if (dialogueText.maxVisibleCharacters < dialogues[currentIdx].Length)
            {
                dialogueText.maxVisibleCharacters++;
                anim.SetBool("talk", true);
            }
            else
            {
                anim.SetBool("talk", false);
                break;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator patienceDrop()
    {
        while (patience > 0)
        {
            patience--;
            yield return new WaitForSeconds(2f);
        }
    }

    public void NextDialogue()
    {
        if (currentIdx < dialogues.Count - 1)
        {
            if (dialogueText.maxVisibleCharacters == dialogues[currentIdx].Length)
            {   
                currentIdx++;
                dialogueText.text = dialogues[currentIdx];


                dialogueText.maxVisibleCharacters = 0;
                StopCoroutine(animate);
                animate = StartCoroutine(animationText());
                //modif
                if (currentIdx == 2)
                {
                    WHATButton.gameObject.SetActive(false);
                    WHATButton1.gameObject.SetActive(true);
                    if (tutor.isTutoring)
                    {
                        WHATButton1.interactable = false;
                    }
                }
                else if (currentIdx >= 3)
                {
                    CustomerLeaves();
                    anim.Play("mad");
                    OKButton.interactable = false;
                    WHATButton.interactable = false;
                    WHATButton1.interactable = false;
                }
                //
            }
        }
        //mod
        else
        {
            mainAnim.SetTrigger("next");
            //dialogueBubble.SetActive(false);
        }
    }

    public void SubmitOrder(string bento)
    {
        if (isWaitingForOrder)
        {
            notes.resetChecklist();
            Animator tipping = GameObject.Find("Tip").transform.GetChild(0).GetComponent<Animator>();
            CultureInfo culture = new CultureInfo("id-ID");
            tipping.GetComponent<TextMeshProUGUI>().text = tips.ToString("C", culture);
            tipping.Play("in");

            //dialogueBubble.SetActive(true);
            if(currentOrder == bento)
            {
                if (tutor.isTutoring && tutor.idx == 19)
                {
                    tutor.nextTut();
                }
                if (patience >= 75)
                {
                    tips = 15f / 100f * price;
                }
                else if(patience < 75 && patience >= 50)
                {
                    tips = 10f / 100f * price;
                }
                else if(patience < 50 && patience >= 25)
                {
                    tips = 5f / 100f * price;
                }
                else if(patience < 25)
                {
                    tips = 0f;
                }
                dialogueText.text = dialogues[dialogues.Count - 1];
                dialogueText.maxVisibleCharacters = 0;
                StopCoroutine(animate);
                animate = StartCoroutine(animationText());
                currencyManager.AddCoins(tips);
                dayReceipt.totalEarnings += tips;
                dayReceipt.customerServed += 1;
                dayReceipt.UpdateSatisfactionRate(patience);
                dayReceipt.UpdateReceiptData();
            }
            else
            {
                anim.Play("mad");
                dialogueText.text = dialogues[dialogues.Count - 2];
                dialogueText.maxVisibleCharacters = 0;
                StopCoroutine(animate);
                animate = StartCoroutine(animationText());
                currencyManager.DecreaseCoins(price);
                dayReceipt.totalEarnings -= price;
                dayReceipt.customerServed += 1;
                dayReceipt.UpdateSatisfactionRate(patience);
                dayReceipt.UpdateReceiptData();
            }
            currentIdx = dialogues.Count - 1;
            isWaitingForOrder = false;
            StartCoroutine(CompleteOrderAfterDelay(3f));
        }
    }

    public void SetCurrentOrder()
    {
        if(orderName == "Soup Set (Mom)")
        {
            currentOrder = "MomSoupBento";
        }
        else if(orderName == "Soup Set (Kid)")
        {
            currentOrder = "ChildSoupBento";
        }
        else if(orderName == "Stir Fry Set (Mom)")
        {
            currentOrder = "MomStirFryBento";
        }
        else if(orderName == "Stir Fry Set (Kid)")
        {
            currentOrder = "ChildStirFryBento";
        }
    }

    public void CustomerLeaves()
    {
        dialogueText.text = dialogues[dialogues.Count - 2].ToString();
        currentIdx = dialogues.Count - 2;
        isWaitingForOrder = false;
        OKButton.interactable = false;
        WHATButton.interactable = false;
        StartCoroutine(CompleteOrderAfterDelay(2f));
    }

    private IEnumerator CompleteOrderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnOrderCompleted?.Invoke();
        //modif
        //dialogueBubble.SetActive(false);
        mainAnim.SetTrigger("next");
        anim.SetTrigger("exit");
        Invoke("leaves", 1.5f);
        //Destroy(gameObject);
        //
    }
    //tambahan
    void leaves()
    {
        Destroy(gameObject);
    }
    //

    private IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueBubble.SetActive(false);
    }

    public void ToKitchen()
    {
        if (tutor.isTutoring && tutor.idx == 4)
        {
            tutor.nextTut();
        }
        OKButton.gameObject.SetActive(false);
        WHATButton.gameObject.SetActive(false);
        WHATButton1.gameObject.SetActive(false);
        currencyManager.AddCoins(price);
        dayReceipt.totalEarnings += price;
        switch_room switch_Room = FindObjectOfType<switch_room>();
        switch_Room.switchRoom(GameObject.Find("Canvas_KITCHEN").GetComponent<Canvas>());
    }
}