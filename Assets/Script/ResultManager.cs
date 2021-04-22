using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ResultManager : MonoBehaviour
{
    public GameObject TYPE_AorC;
    public GameObject TYPE_B;

    private void Start()
    {
        //前回のシーンがtest2の時TYPE_AorCを非表示にする
        if (test2.oldSceneName != null)
        {
            TYPE_B.SetActive(true);
            TYPE_AorC.SetActive(false);
            
        }
        //前回のシーンがtest3orIsha_Syngleshotの時TYPE_Bを非表示にする
        else 
        {
            TYPE_AorC.SetActive(true);
            TYPE_B.SetActive(false);
            
        }

    }
}
