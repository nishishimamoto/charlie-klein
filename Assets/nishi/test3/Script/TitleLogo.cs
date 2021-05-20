using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogo : MonoBehaviour
{
    [SerializeField] GameObject titleLogo;
    [SerializeField] GameObject buttons;
    float logoSize;
    float animTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        titleLogo.transform.localScale = new Vector3(394, 0, 1);
        buttons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        animTime += Time.deltaTime;
        if (logoSize < 218 && animTime >= 0.5f)
        {
            logoSize = animTime * 130;
            if (logoSize > 218) logoSize = 218;
            titleLogo.transform.localScale = new Vector3(394, logoSize, 1);
        }
        else if(animTime >= 1.8f)
        {
            buttons.SetActive(true);
        }
    }
}
