using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CustomerSpawner : MonoBehaviour
{
    //modif
    [SerializeField] private List<Sprite> customerSprites;
    //
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private order_notes order_note;
    [Serializable] public struct CustomerDialogues
    {
        //indonesia sm b.ing
        [TextArea] public List<string> dialogueIND;
        [TextArea] public List<string> dialogueENG;
    };
    [SerializeField] private CustomerDialogues[] customerDialogues;
    [SerializeField] private CustomerDialogues currentCustomerDialogues;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Canvas canvas;
    // [SerializeField] private float spawnInterval = 5f;
    [Serializable] public struct Orders
    {
        public string orderName_IND;
        public string orderName_ENG;
        public List<string> ingredientsIND;
        public List<string> ingredientsENG;
        [TextArea] public List<string> stepsIND;
        [TextArea] public List<string> stepsENG;
        public List<string> toolsENG;
        public List<string> toolsIND;
        public float price;
    }
    [SerializeField] private Orders[] orders;
    public Orders currentOrder;
    [SerializeField] private GameObject currentCustomer;

    private void Start()
    {
        // order_note = FindObjectOfType<order_notes>();
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            //modif
            if (customerSprites.Count == 0)
            {
                Debug.LogError("No customer prefabs assigned!");
                yield break;
            }
            //

            int randomIdxSprite = UnityEngine.Random.Range(0, customerSprites.Count);
            int randomIdxDialogue = UnityEngine.Random.Range(0, customerDialogues.Length);
            //int randomIdxOrder = UnityEngine.Random.Range(0, orders.Length);
            //kt samain length dialog sm ordernya, jd kt cmn nyari random idx dialogue aja, gk perlu ordernya jg

            currentCustomerDialogues = customerDialogues[randomIdxDialogue];
            currentOrder = orders[randomIdxDialogue];
            //currentOrder = orders[randomIdxOrder];
            //revisi, kt samain aja index dialog sm ordernya, soalnya kl dibedain nnt ordernya gk sesuai sm dialognya :)

            //modif
            //GameObject customer = Instantiate(customerPrefab, canvas.transform);
            GameObject customer = Instantiate(customerPrefab, spawnPoint);
            customer.GetComponent<RectTransform>().anchorMin = Vector3.zero;
            customer.GetComponent<RectTransform>().anchorMax = Vector3.one;
            customer.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            customer.GetComponent<RectTransform>().sizeDelta = Vector2.zero;


            //customer.transform.localScale = Vector3.one;
            //customer.transform.SetParent(canvas.transform, false);

            customer.transform.GetChild(0).GetComponent<Image>().sprite = customerSprites[randomIdxSprite];

            if (PlayerPrefs.GetString("language") == "Indonesia")
            {
                customer.GetComponent<Customer>().dialogues = currentCustomerDialogues.dialogueIND;
                customer.GetComponent<Customer>().orderName = currentOrder.orderName_IND;
                customer.GetComponent<Customer>().ingredients = currentOrder.ingredientsIND;
                order_note.SetOrder(currentOrder.stepsIND, currentOrder.ingredientsIND, currentOrder.toolsIND);
            }
            else if(!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "English")
            {
                customer.GetComponent<Customer>().dialogues = currentCustomerDialogues.dialogueENG;
                customer.GetComponent<Customer>().orderName = currentOrder.orderName_ENG;
                customer.GetComponent<Customer>().ingredients = currentOrder.ingredientsENG;
                order_note.SetOrder(currentOrder.stepsENG, currentOrder.ingredientsENG, currentOrder.toolsENG);
            }
            //
            customer.GetComponent<Customer>().price = currentOrder.price;
            customer.GetComponent<Customer>().OnOrderCompleted += HandleOrderCompleted;
            currentCustomer = customer;
            // customer.GetComponent<Customer>().Initialize();

            yield return new WaitUntil(() => currentCustomer == null);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void HandleOrderCompleted()
    {
        currentCustomer = null;
    }

    // public void SubmitOrderNow()
    // {
    //     if (currentCustomer != null)
    //     {
    //         currentCustomer.GetComponent<Customer>().SubmitOrder();
    //     }
    // }
}