using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    RectTransform self;
    Vector3 pos;
    float blinking = 0.5f;
    [SerializeField] float blinkingSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<RectTransform>();
        pos = self.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        blinking = Mathf.Sin(Mathf.PI * blinkingSpeed * Time.time); //sin波取得 点滅
        //self.position = new Vector3(blinking * 5, 0, 0);
        pos.x += blinking / 10;
        self.anchoredPosition = pos;
    }
}
