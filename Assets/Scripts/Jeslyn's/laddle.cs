using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laddle : MonoBehaviour
{
    [SerializeField] private stir aduk;
    [SerializeField] private GameObject laddle_in;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = laddle_in.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = rb.transform.position;
        if (aduk.isDragging && Vector2.Distance(laddle_in.GetComponent<RectTransform>().position, Input.mousePosition) > 1f)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (Input.mousePosition - laddle_in.GetComponent<RectTransform>().position).normalized;
            rb.velocity = direction * 2000f;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
