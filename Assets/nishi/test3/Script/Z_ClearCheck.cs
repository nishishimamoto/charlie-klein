using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_ClearCheck : MonoBehaviour
{
    const int stageNum = 10;
    [SerializeField] GameObject[] stage = new GameObject[stageNum];
    public static bool[] isClear = new bool [stageNum];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < stageNum; i++)
        {
            stage[i].SetActive(false);
            if (isClear[i]) stage[i].SetActive(true);
            Debug.Log(isClear[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
