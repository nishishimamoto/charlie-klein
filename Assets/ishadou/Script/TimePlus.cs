using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePlus : MonoBehaviour
{
    [SerializeField] Isha_Singlshot ishaCS;
    [SerializeField] Text TimeText;

    public float DrawTime;
    private Color TpStartColor;
    private Color TpLastColor;
    // Start is called before the first frame update
    void Start()
    {
        TimeText.gameObject.SetActive(false);
        TpStartColor = TimeText.color;
        TpLastColor = new Color(0.3f, 0.5f, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TimePlusDraw()
    {
        TimeText.text = "+" + " " + ishaCS.TimePlusNum + "s";

        TimeText.gameObject.SetActive(true);

        StartCoroutine(nameof(TimeFade));
    }

    private IEnumerator TimeFade()
    {
        float waitTime = 0;
        float rate = 0;
        while (waitTime <= DrawTime)
        {
            rate = waitTime;
            waitTime += Time.deltaTime;
            TimeText.color = Color.Lerp(TpStartColor, TpLastColor, rate);
            yield return new WaitForFixedUpdate();
        }
        TimeText.gameObject.SetActive(false);
        TimeText.color = TpStartColor;
    }
}
