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
        [TextArea] public List<string> dialogue;
    };
    [SerializeField] private CustomerDialogues[] customerDialogues;
    [SerializeField] private CustomerDialogues currentCustomerDialogues;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Canvas canvas;
    // [SerializeField] private float spawnInterval = 5f;
    [Serializable] public struct Orders
    {
        public string orderName;
        public List<string> ingredients;
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
            
            GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            customer.transform.SetParent(canvas.transform, false);

            customer.GetComponent<Image>().sprite = customerSprites[randomIdxSprite];
            customer.GetComponent<Customer>().dialogues = currentCustomerDialogues.dialogue;
            customer.GetComponent<Customer>().orderName = currentOrder.orderName;
            customer.GetComponent<Customer>().ingredients = currentOrder.ingredients;
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