using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public void SelectImageMove(int chooseMain)
    {
        GetComponent<RectTransform>().anchoredPosition
            = new Vector2(-6.5f + (2 * (chooseMain % 6)), 4.5f - (2 * (chooseMain / 6)) -0.5f);
    }
}
