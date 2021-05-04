using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassBox : MonoBehaviour
{
    [SerializeField] GameObject massSprite;
    GameObject[] boxSprite;
    public bool[] isBox;
    public bool[] isMassSE;
    public int[] massColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MassInit(int main) //値を受け取って初期化
    {
        boxSprite = new GameObject[main];
        isBox = new bool [main];
        isMassSE = new bool[main];
        massColor = new int[main];

        for (int i = 0; i < main; i++)
        {
            boxSprite[i] = Instantiate(massSprite, new Vector3(-6 + (2 * (i % 6)), 4 - (2 * (i / 6)), 0), Quaternion.identity);
            boxSprite[i].SetActive(false);
        }
    }

    public void Mass(int main)
    {
        for(int i = 0; i < main; i++)
        {
            if (isBox[i] && !boxSprite[i].activeSelf) boxSprite[i].SetActive(true);
            else if (!isBox[i] && boxSprite[i].activeSelf) boxSprite[i].SetActive(false);
            MassColorChange(i); //色を変える
        }
    }

    public void MassDelete(int main)
    {
        for (int i = 0; i < main; i++)
        {
            boxSprite[i].SetActive(false);
            isBox[i] = false;
            isMassSE[i] = false;
        }
    }

    void MassColorChange(int i)
    {
        switch (massColor[i])
        {
            case 1:
                if(boxSprite[i].GetComponent<SpriteRenderer>().color != Color.cyan) boxSprite[i].GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case 8:
                if (boxSprite[i].GetComponent<SpriteRenderer>().color != Color.red) boxSprite[i].GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 32:
                if (boxSprite[i].GetComponent<SpriteRenderer>().color != Color.yellow) boxSprite[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 128:
                if (boxSprite[i].GetComponent<SpriteRenderer>().color != Color.blue) boxSprite[i].GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            default:
                break;
        }
    }
}
