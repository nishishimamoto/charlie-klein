using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursolColor : MonoBehaviour
{
    [SerializeField] Bom bom;
    SpriteRenderer sprite;

    float alpha = 200;
    public int al_Max;
    public int al_Min;
    public float al_Add;
    bool al_fl = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.name == "Hexegram")
        {
            if (al_fl)
            {
                alpha += al_Add;
                if (alpha > al_Max) al_fl = false;
            }
            else if (!al_fl)
            {
                alpha -= al_Add;
                if (alpha < al_Min) al_fl = true;
            }

            if ((bom.bomGauge >= bom.maxBomGauge) && sprite.color != Color.red) sprite.color = sprite.color = new Color(1, 0, 0, alpha / 255);
            else if ((bom.bomGauge < bom.maxBomGauge) && sprite.color != Color.white) sprite.color = new Color(1, 1, 1, alpha / 255);
        }
        else if ((bom.bomGauge >= bom.maxBomGauge) && sprite.color != Color.red) sprite.color = Color.red;
        else if((bom.bomGauge < bom.maxBomGauge) && sprite.color != Color.white) sprite.color = Color.white;
    }
}
