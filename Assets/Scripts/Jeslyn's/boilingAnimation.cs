using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boilingAnimation : MonoBehaviour
{
    [SerializeField] private stir aduk;
    [SerializeField] private List<Image> ingredientsInPot;
    [SerializeField] private List<Image> animation;
/*    private Vector2 centerScreenPos;
    private Vector2 previousDirection;
    private RectTransform rectTransform;*/
    // Start is called before the first frame update
    void Start()
    {
        //rectTransform = animation[2].GetComponent<RectTransform>();
        resetColor();
    }

    // Update is called once per frame
    void Update()
    {
        /*        if (aduk.isDragging)
                {
                    Vector2 currentMousePos = Input.mousePosition;
                    Vector2 currentDirection = currentMousePos - centerScreenPos;

                    if (currentDirection.magnitude > 0.01f) // Prevents NaN or weird values
                    {
                        float angleDelta = Vector2.SignedAngle(previousDirection, currentDirection);
                        rectTransform.Rotate(0f, 0f, angleDelta);
                        previousDirection = currentDirection;
                    }
                }*/
        animation[2].GetComponent<RectTransform>().rotation = aduk.GetComponent<RectTransform>().rotation;
        if (aduk.stir_value > 0 && aduk.stir_value < 100)
        {
            Color color1 = new Color(1,1,1,1-aduk.stir_value/100);
            Color color2 = new Color(1,1,1,aduk.stir_value/100);
            animation[0].color = color1;
            animation[1].color = color2;
            animation[2].color = color2;
            animation[3].color = Color.clear;
        }
    }
    public void doneCooking()
    {
        animation[0].color = Color.clear;
        animation[1].color = Color.clear;
        animation[2].color = Color.clear;
        animation[3].color = Color.white;
        Invoke("resetColor", 1.2f);
    }
    void resetColor()
    {
        animation[0].color = Color.white;
        animation[1].color = Color.clear;
        animation[2].color = Color.clear;
        animation[3].color = Color.clear;
    }
}
