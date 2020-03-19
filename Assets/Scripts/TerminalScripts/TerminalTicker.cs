using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalTicker : MonoBehaviour
{
    public TerminalTicker instance;
    public TextMeshProUGUI tickerText;
    public float scrollSpeed;
    public GameObject startingPoint;

    private TextMeshProUGUI closeTickerText;
    private RectTransform textRectTransform;

    private static string tickerMessage;
    private static float width;

    private void Awake()
    {
        instance = this;
        textRectTransform = tickerText.GetComponent<RectTransform>();
        UpdateText("Hacker Box Version 1.8 is available");
        StartCoroutine(StartScroll());
    }

    public void UpdateText(string message)
    {
        tickerMessage = message;
    }

    IEnumerator StartScroll()
    {        
        string currentMessage = tickerMessage;
        Vector2 startPos = startingPoint.transform.localPosition;
        float scrollPos = startPos.x;

        while (true)
        {
            if (currentMessage != tickerMessage)
            {
                width = tickerText.preferredWidth;
                currentMessage = tickerText.text;
            }

            tickerText.text = tickerMessage;
            width = tickerText.preferredWidth;

            if (scrollPos <= -width)
            {
                scrollPos = startPos.x;
            }

            textRectTransform.localPosition = new Vector2(scrollPos, startPos.y);
            scrollPos -= scrollSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
