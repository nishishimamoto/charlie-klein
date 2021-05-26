using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG_Flash_1st : MonoBehaviour
{
    [SerializeField] Isha_Singlshot ishaCS;
    [SerializeField] GameSE gameSECS;

    Image MgImage;

    float nowFlashTime;
    public float MgSwitchTime;

    private Color MgStartColor;
    private Color MgFlashColor;

    bool isFlash;

    void Start()
    {
        MgImage = this.gameObject.GetComponent<Image>();
        MgStartColor = this.gameObject.GetComponent<Image>().color;
        MgFlashColor = new Color32(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (ishaCS.BonusFlg == 1)
        {
            if (ishaCS.BonusGaugeSand.fillAmount <= 0.3f)
            {
                if (!isFlash)
                {
                    isFlash = true;
                    StartCoroutine(nameof(MgBlinking));
                    gameSECS.audioSource.PlayOneShot(gameSECS.pause);
                }
            }
        }
        else
        {
            isFlash = false;
        }
    }

    private IEnumerator MgBlinking()
    {
        float waitTime = MgSwitchTime;
        nowFlashTime = 0;
        while (waitTime > nowFlashTime)
        {
            nowFlashTime += Time.deltaTime;
            float rate = nowFlashTime;

            MgImage.color = Color.Lerp(MgStartColor, MgFlashColor, rate);
            yield return new WaitForFixedUpdate();
        }

        nowFlashTime = 0;
        while (waitTime > nowFlashTime)
        {
            nowFlashTime += Time.deltaTime;
            float rate = nowFlashTime * 2f;

            MgImage.color = Color.Lerp(MgFlashColor, MgStartColor, rate);
            yield return new WaitForFixedUpdate();
        }
        isFlash = false;
    }
}
