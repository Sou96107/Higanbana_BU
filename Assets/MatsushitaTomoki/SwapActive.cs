using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapActive : MonoBehaviour
{
    public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 非アクティブにする関数
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    // アクティブにする関数
    public void ActivateObject()
    {
        gameObject.SetActive(true);
    }
}
