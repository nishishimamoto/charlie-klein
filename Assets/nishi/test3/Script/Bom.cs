using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bom : MonoBehaviour
{
    [SerializeField] Pause pauseCS;
    [SerializeField] MobiusCreate mobiusCS;
    public float bomGauge = 0;
    public float maxBomGauge = 15000;
    public bool isBomUse;
    [SerializeField] GameObject bomSlider;
    Image bomGaugeImg;
    [SerializeField] GameObject bomText;
    Rot_Timer rot_Timer;
    [SerializeField] GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        bomGaugeImg = bomSlider.GetComponent<Image>();
        rot_Timer = bomSlider.GetComponent<Rot_Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bomGauge > maxBomGauge) bomGauge = maxBomGauge;

        if(pauseCS.isPause) bomText.SetActive(false);
        else if (bomGauge == maxBomGauge && !bomText.activeSelf)
        {
            bomGaugeImg.color = Color.red;
            bomText.SetActive(true);
            particle.SetActive(true);
            rot_Timer.speed = 0.1f;
        }
        else if (bomGauge < maxBomGauge && bomText.activeSelf)
        {
            bomGaugeImg.color = Color.white;
            bomText.SetActive(false);
            particle.SetActive(false);
            rot_Timer.speed = 0;
            bomSlider.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        bomGaugeImg.fillAmount = bomGauge / maxBomGauge;    //円画像の表示
    }
}
