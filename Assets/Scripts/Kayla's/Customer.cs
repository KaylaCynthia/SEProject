using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    public List<string> dialogues;
    public string orderName;
    public List<string> ingredients;
    public float price;
    public string currentOrder;
    private TextMeshProUGUI dialogueText;
    private GameObject dialogueBubble;
    private int currentIdx;
    private bool isWaitingForOrder = false;

    public delegate void OrderCompleted();
    public event OrderCompleted OnOrderCompleted;

    private void Start()
    {
        dialogueBubble = transform.Find("DialogueBubble").gameObject;
        dialogueText = dialogueBubble.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        StartDialogue();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!isWaitingForOrder)
            {
                NextDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        currentIdx = 0;
        dialogueBubble.SetActive(true);
        dialogueText.text = dialogues[currentIdx];
        if (currentIdx == dialogues.Count - 2)
        {
            isWaitingForOrder = true;
            StartCoroutine(CloseDialogue());
        }
    }

    public void NextDialogue()
    {
        if (currentIdx < dialogues.Count - 1)
        {
            currentIdx++;
            dialogueText.text = dialogues[currentIdx];

            if (currentIdx == dialogues.Count - 2)
            {
                isWaitingForOrder = true;
            }
        }
        else
        {
            dialogueBubble.SetActive(false);
        }
    }

    public void SubmitOrder()
    {
        Debug.Log("This is passed");
        if (isWaitingForOrder)
        {
            dialogueBubble.SetActive(true);
            dialogueText.text = dialogues[dialogues.Count - 1].ToString();
            currentIdx++;
            isWaitingForOrder = false;
            StartCoroutine(CompleteOrderAfterDelay(2f));
        }
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