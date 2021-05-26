using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePlus : MonoBehaviour
{
    [SerializeField] Isha_Singlshot ishaCS;
    [SerializeField] Text TimeText;

    public float DrawTime;
    // Start is called before the first frame update
    void Start()
    {
        TimeText.gameObject.SetActive(false);
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

        while (waitTime <= DrawTime)
        {
            waitTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        TimeText.gameObject.SetActive(false);
    }
}
