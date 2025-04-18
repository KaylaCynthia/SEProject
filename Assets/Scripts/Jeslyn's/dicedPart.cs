using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dicedPart : MonoBehaviour
{
    public Vector2 target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime*5f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * 5f);
    }
}
