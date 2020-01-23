using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalTicker : MonoBehaviour
{
    public TextMeshProUGUI tickerText;
    public float scrollSpeed;

    private TextMeshProUGUI closeTickerText;
    private RectTransform textRectTransform;

    private void Awake()
    {
        textRectTransform = tickerText.GetComponent<RectTransform>();
        StartCoroutine(StartScroll());
    }

    IEnumerator StartScroll()
    {
        float width = tickerText.preferredWidth / 2;
        Vector2 startPos = textRectTransform.localPosition;

        float scrollPos = -100;

        while (true)
        {
            if((textRectTransform.localPosition.x + 100f) <= -width)
            {
                scrollPos = -100;
            }

            textRectTransform.localPosition = new Vector2(-scrollPos, startPos.y);

            scrollPos += scrollSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
