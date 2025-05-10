using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
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
    private GameObject dialogueBubble;
    private int currentIdx;
    private bool isWaitingForOrder = false;

    public delegate void OrderCompleted();
    public event OrderCompleted OnOrderCompleted;
    private CurrencyManager currencyManager;
    private DayReceipt dayReceipt;

    private void Start()
    {
        patienceText = GameObject.Find("notes").transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        dayReceipt = FindObjectOfType<DayReceipt>();
        dialogueBubble = transform.Find("DialogueBubble").gameObject;
        dialogueText = dialogueBubble.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        StartDialogue();
        StartCoroutine(patienceDrop());
        SetCurrentOrder();
    }

    Coroutine animate;
    bool isLeaving = false;
    
    private void Update()
    {
        patienceText.text = patience.ToString() + "%";
        if (!isLeaving && currentIdx == dialogues.Count - 2 && dialogueText.maxVisibleCharacters == dialogues[currentIdx].Length)
        {
            isLeaving = true;
            CustomerLeaves();
        }
    }

    public void StartDialogue()
    {
        dialogueBubble.SetActive(true);
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
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator patienceDrop()
    {
        while (patience > 0)
        {
            patience--;
            yield return new WaitForSeconds(1f);
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

                if (currentIdx == dialogues.Count - 3)
                {
                    OKButton.interactable = false;
                    WHATButton.interactable = false;
                }
            }

        }
        else
        {
            dialogueBubble.SetActive(false);
        }
    }

    public void SubmitOrder(string bento)
    {
        if (isWaitingForOrder)
        {
            tips = 5 / 100 * price;
            dialogueBubble.SetActive(true);
            if(currentOrder == bento)
            {
                dialogueText.text = dialogues[dialogues.Count - 1].ToString();
                currencyManager.AddCoins(tips);
                dayReceipt.totalEarnings += tips;
                dayReceipt.customerServed += 1;
                dayReceipt.UpdateSatisfactionRate(patience);
                dayReceipt.UpdateReceiptData();
            }
            else
            {
                dialogueText.text = dialogues[dialogues.Count - 2].ToString();
                currencyManager.DecreaseCoins(price);
                dayReceipt.totalEarnings -= price;
                dayReceipt.customerServed += 1;
                dayReceipt.UpdateSatisfactionRate(patience);
                dayReceipt.UpdateReceiptData();
            }
            currentIdx = dialogues.Count - 1;
            isWaitingForOrder = false;
            StartCoroutine(CompleteOrderAfterDelay(2f));
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
        else
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
        Destroy(gameObject);
    }

    private IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueBubble.SetActive(false);
    }

    public void ToKitchen()
    {
        OKButton.gameObject.SetActive(false);
        WHATButton.gameObject.SetActive(false);
        currencyManager.AddCoins(price);
        dayReceipt.totalEarnings += price;
        switch_room switch_Room = FindObjectOfType<switch_room>();
        switch_Room.switchRoom(GameObject.Find("Canvas_KITCHEN").GetComponent<Canvas>());
    }
}