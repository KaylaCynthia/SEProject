using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    private TextMeshProUGUI patienceText;
    public float patience = 100f;
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
        // patienceText = GameObject.Find("PatienceText").GetComponent<TextMeshProUGUI>();
        dialogueBubble = transform.Find("DialogueBubble").gameObject;
        dialogueText = dialogueBubble.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        OKButton = transform.Find("OKButton").GetComponent<Button>();
        WHATButton = transform.Find("WHATButton").GetComponent<Button>();
        StartDialogue();
    }

    //tambahan
    Coroutine animate;
    bool isLeaving = false;
    //
    private void Update()
    {
        // patienceText.text = patience.ToString() + "%";
        //tambahan - customer baru leave kl dialognya dah kelar
        if (!isLeaving && currentIdx == dialogues.Count - 2 && dialogueText.maxVisibleCharacters == dialogues[currentIdx].Length)
        {
            isLeaving = true;
            CustomerLeaves();
        }
        //
    }

    public void StartDialogue()
    {
        dialogueBubble.SetActive(true);

        //tambahan
        dialogueText.maxVisibleCharacters = 0;
        //

        dialogueText.text = dialogues[currentIdx];

        //tambahan
        animate = StartCoroutine(animationText());
        //

        isWaitingForOrder = true;
        OKButton.interactable = true;
        WHATButton.interactable = true;
        // StartCoroutine(CloseDialogue());
    }

    //tambahan - animasi text dialog
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
    //

    public void NextDialogue()
    {
        if (currentIdx < dialogues.Count - 1)
        {
            //tambahan
            if (dialogueText.maxVisibleCharacters == dialogues[currentIdx].Length)
            {   
                currentIdx++;
                dialogueText.text = dialogues[currentIdx];


                dialogueText.maxVisibleCharacters = 0;
                StopCoroutine(animate);
                animate = StartCoroutine(animationText());
                //

                if (currentIdx == dialogues.Count - 2)
                {
                    //ku comment ini biar customer itu leave pas dialognya kelar dlu
                    //CustomerLeaves();
                
                    //ganti jd ini
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