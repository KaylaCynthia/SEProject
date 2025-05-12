using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cust_anim : MonoBehaviour
{
    [SerializeField] private Customer customer;
    bool ordered = false;
    // Start is called before the first frame update
    void Start()
    {
        customer = transform.parent.GetComponent<Customer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void order()
    {
        if (!ordered)
        {
            ordered = true;
            customer.startOrdering();
        }
    }

    public void leave()
    {
        Destroy(gameObject);
    }
}
