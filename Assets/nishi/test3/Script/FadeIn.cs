using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    SpriteRenderer selfSprite;
    float blinking = 0;
    // Start is called before the first frame update
    void Start()
    {
        selfSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blinking <= 1)
        {
            blinking += Time.deltaTime * 1.3f;
            selfSprite.color = new Color(1, 1, 1, blinking);  //絶対値でsin波を透明度に 点滅
        }

        if (!selfSprite.enabled) blinking = 0;
    }
}
