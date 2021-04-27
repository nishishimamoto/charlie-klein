using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassBox : MonoBehaviour
{
    [SerializeField] GameObject massSprite;
    GameObject[] boxSprite;
    public bool[] isBox;
    public bool[] isMassSE;
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

        for (int i = 0; i < main; i++)
        {
            boxSprite[i] = Instantiate(massSprite, new Vector3(-6 + (2 * (i % 6)), 4 - (2 * (i / 6)), 2.0f), Quaternion.identity);
            boxSprite[i].SetActive(false);
        }
    }

    public void Mass(int main)
    {
        for(int i = 0; i < main; i++)
        {
            if (isBox[i] && !boxSprite[i].activeSelf) boxSprite[i].SetActive(true);
            else if (!isBox[i] && boxSprite[i].activeSelf) boxSprite[i].SetActive(false);
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
}
