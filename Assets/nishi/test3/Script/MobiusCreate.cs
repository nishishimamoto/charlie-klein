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
    [SerializeField] GameObject ring;
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

    //public void RingCreate(int x,int y)
    //{
    //    GameObject left = Instantiate(ring, new Vector3(-6 + (2 * (x % 6)), 4 + (-2 * (x / 6)), 0), Quaternion.identity);
    //    Instantiate(ring, new Vector3(-6 + (2 * (y % 6)), 4 + (-2 * (y / 6)), 0), Quaternion.identity);
    //    left.transform.Rotate(0f, 180f, 0f);
    //}
}
