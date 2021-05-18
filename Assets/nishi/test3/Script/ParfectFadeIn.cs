using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParfectFadeIn : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] perfectImage;
    public float blinkig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blinkig < 1)
        {
            blinkig += Time.deltaTime;

            perfectImage[0].color = new Color(1, 0, 1, blinkig);
            perfectImage[1].color = new Color(1, 0, 1, blinkig);
            perfectImage[2].color = new Color(1, 0, 1, blinkig);
        }
        else if (blinkig > 1)
        {
            blinkig = 1;
        }
    }
}
