using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoControll : MonoBehaviour
{
    public GameObject objectToExclude; // 動きを止めないオブジェクト
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != objectToExclude)
        {
            // 特定のオブジェクト以外の場合、動きを停止する
            return;
        }
    }
}
