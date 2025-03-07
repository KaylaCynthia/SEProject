using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    //disini dialognya 1 aja gpp, nnt ganti bahasanya itu cmn bs di main menu soalnya
    public List<string> dialogues;
    public string orderName;
    public List<string> ingredients;
    public float price;
    public string currentOrder;
    private TextMeshProUGUI dialogueText;
    private Button OKButton;
    private Button WHATButton;
    private GameObject dialogueBubble;
    private int currentIdx;
    private bool isWaitingForOrder = false;

    public delegate void OrderCompleted();
    public event OrderCompleted OnOrderCompleted;

    private void Start()
    {
        dialogueBubble = transform.Find("DialogueBubble").gameObject;
        dialogueText = dialogueBubble.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        OKButton = transform.Find("OKButton").GetComponent<Button>();
        WHATButton = transform.Find("WHATButton").GetComponent<Button>();
        StartDialogue();
    }

    private void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     if (!isWaitingForOrder)
        //     {
        //         NextDialogue();
        //     }
        // }
    }

    public void StartDialogue()
    {
        currentIdx = 0;
        dialogueBubble.SetActive(true);
        dialogueText.text = dialogues[currentIdx];
        isWaitingForOrder = true;
        OKButton.interactable = true;
        WHATButton.interactable = true;
        // StartCoroutine(CloseDialogue());
    }

    public void NextDialogue()
    {
        if (currentIdx < dialogues.Count - 1)
        {
            currentIdx++;
            dialogueText.text = dialogues[currentIdx];

            if (currentIdx == dialogues.Count - 2)
            {
                CustomerLeaves();
            }
        }
        else
        {
            dialogueBubble.SetActive(false);
        }
    }

    public void SubmitOrder()
    {
        if (isWaitingForOrder)
        {
            OKButton.interactable = false;
            WHATButton.interactable = false;
            dialogueBubble.SetActive(true);
            dialogueText.text = dialogues[dialogues.Count - 1].ToString();
            currentIdx = dialogues.Count - 1;
            isWaitingForOrder = false;
            StartCoroutine(CompleteOrderAfterDelay(2f));
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
}