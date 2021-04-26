using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOver : MonoBehaviour
{
    const int maxlife = 4;
    public int lifeSpan;
    float lifeBeat = 1.0f;
    float beatSpeed;
    [SerializeField] GameObject satelite;
    // Start is called before the first frame update
    void Start()
    {
        lifeSpan = maxlife;
        GetComponent<TextMesh>().text = "" + lifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        switch (lifeSpan)
        {
            case 1:
                beatSpeed = 2.0f;
                break;
            case 2:
                beatSpeed = 1.0f;
                break;
            default:
                beatSpeed = 0;
                break;
        }

        lifeBeat = Mathf.Sin(Mathf.PI * beatSpeed * Time.time); //sin波取得 点滅
        transform.localScale = new Vector3(1 + (Mathf.Abs(lifeBeat) / 8), 1 + (Mathf.Abs(lifeBeat) / 8), 1);
        satelite.transform.localScale = new Vector3(1 + (Mathf.Abs(lifeBeat) / 8), 1 + (Mathf.Abs(lifeBeat) / 8), 1);

    }

    public void LifeCountDown()
    {
        lifeSpan -= 1;

        if (lifeSpan == 2) GetComponent<TextMesh>().color = Color.yellow;//黄
        else if (lifeSpan == 1) GetComponent<TextMesh>().color = Color.red;//赤

        GetComponent<TextMesh>().text = "" + lifeSpan;
    }

    public void LifeCountReSet()
    {
        lifeSpan = maxlife;
        GetComponent<TextMesh>().color = Color.white;//白
        GetComponent<TextMesh>().text = "" + lifeSpan;
        transform.localScale = new Vector3(1, 1, 1);
    }
}
