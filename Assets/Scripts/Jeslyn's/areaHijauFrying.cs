using UnityEngine;
using UnityEngine.UI;

public class areaHijauFrying : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform background;      // Ini RectTransform dari Background
    [SerializeField] private RectTransform minMaxTimeBox;   // Ini RectTransform dari MinMaxTime

    [SerializeField] private CookingPot fry;
    [SerializeField] private float rangeStart;
    [SerializeField] private float rangeEnd;

    private void Start()
    {
        background = transform.parent.GetComponent<RectTransform>();
        minMaxTimeBox = GetComponent<RectTransform>();
        rangeStart = fry.minPerfectCook;
        rangeEnd = fry.maxPerfectCook;
    }
    void Update()
    {
        float min = slider.minValue;
        float max = slider.maxValue;
        float totalWidth = background.rect.width;

        float startPercent = (rangeStart - min) / (max - min);
        float endPercent = (rangeEnd - min) / (max - min);

        float xPos = startPercent * totalWidth;
        float width = (endPercent - startPercent) * totalWidth;

        minMaxTimeBox.anchoredPosition = new Vector2(xPos, minMaxTimeBox.anchoredPosition.y);
        minMaxTimeBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
