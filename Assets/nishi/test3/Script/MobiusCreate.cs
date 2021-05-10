using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobiusCreate : MonoBehaviour
{
    public bool isMobius;
    GameObject mobius;  //全消しでメビウスをだす
    [SerializeField] GameObject mobiusCanvas;
    Transform mobiusCanvasTransform;

    // Start is called before the first frame update
    void Start()
    {
        mobius = (GameObject)Resources.Load("Mobius");
        mobiusCanvasTransform = mobiusCanvas.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMobius) Mobius();
    }

    public void Mobius()
    {
        GameObject combos = Instantiate(mobius, new Vector3(0, 0, 0), Quaternion.identity);
        combos.transform.SetParent(mobiusCanvasTransform, false);
        isMobius = false;
    }
}
