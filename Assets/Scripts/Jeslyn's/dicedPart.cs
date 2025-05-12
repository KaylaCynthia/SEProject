using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dicedPart : MonoBehaviour
{
    public Vector2 target;
    Image img;
    public bool destroying = false;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime*5f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 5f);
        if (img != null)
        {
            if(!destroying) img.color = Color.Lerp(img.color, Color.white, Time.deltaTime * 5f);
            else img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * 5f);
        }
        else
        {
            img = GetComponent<Image>();
        }
    }
}
