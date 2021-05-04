using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bom : MonoBehaviour
{
    public float bomGauge = 0;
    public float maxBomGauge = 20000;
    [SerializeField] Image bomSlider;
    [SerializeField] GameObject bomText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bomGauge > maxBomGauge) bomGauge = maxBomGauge;
        if (bomGauge == maxBomGauge && !bomText.activeSelf)
        {
            bomSlider.color = Color.red;
            bomText.SetActive(true);
        }
        else if (bomGauge < maxBomGauge && bomText.activeSelf)
        {
            bomSlider.color = Color.white;
            bomText.SetActive(false);
        }

        bomSlider.fillAmount = bomGauge / maxBomGauge;    //円画像の表示
    }
}
