using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugOpen : MonoBehaviour
{
    Process proc;

    // Start is called before the first frame update
    void Start()
    {
        proc = new Process();
        proc.StartInfo.FileName = "notepad";
        proc.Start();
    }

    // アプリ終了時に呼ばれる
    private void OnApplicationQuit()
    {
        // 別アプリ終了処理

        if (!proc.HasExited)
        {
            // 別アプリが起動中の場合のみ終了させる
            proc.CloseMainWindow();
        }

        proc.Close();
        proc = null;
    }
}
