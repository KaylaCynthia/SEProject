using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
// using System.Diagnostics;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<Sprite> customerSprites;
    [SerializeField] private GameObject customerPrefab;
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
        public float price;
    }
    [SerializeField] private Orders[] orders;
    [SerializeField] private Orders currentOrder;
    [SerializeField] private GameObject currentCustomer;

    private void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    private void Update()
    {
        //di aku bisa kok, mungkin unitymu lg ngebug kali :')
        //btw ku bru nyadar, kt kan nnt mau di mobile. Code ini km mau pake buat apa? yg input spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hehe");
            if (currentCustomer != null)
            {
                currentCustomer.GetComponent<Customer>().SubmitOrder();
            }
        }
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            if (customerSprites.Count == 0)
            {
                Debug.LogError("No customer prefabs assigned!");
                yield break;
            }

            int randomIdxSprite = UnityEngine.Random.Range(0, customerSprites.Count);
            int randomIdxDialogue = UnityEngine.Random.Range(0, customerDialogues.Length);
            int randomIdxOrder = UnityEngine.Random.Range(0, orders.Length);

            currentCustomerDialogues = customerDialogues[randomIdxDialogue];
            currentOrder = orders[randomIdxOrder];
            
            GameObject customer = Instantiate(customerPrefab, canvas.transform);
            //customer.transform.localScale = Vector3.one;
            //customer.transform.SetParent(canvas.transform, false);

            customer.GetComponent<Image>().sprite = customerSprites[randomIdxSprite];
            if (!PlayerPrefs.HasKey("language") || PlayerPrefs.GetString("language") == "Indonesia")
            {
                customer.GetComponent<Customer>().dialogues = currentCustomerDialogues.dialogueIND;
                customer.GetComponent<Customer>().orderName = currentOrder.orderName_IND;
                customer.GetComponent<Customer>().ingredients = currentOrder.ingredientsIND;
            }
            else
            {
                customer.GetComponent<Customer>().dialogues = currentCustomerDialogues.dialogueENG;
                customer.GetComponent<Customer>().orderName = currentOrder.orderName_ENG;
                customer.GetComponent<Customer>().ingredients = currentOrder.ingredientsENG;
            }

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
}