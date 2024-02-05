using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushUI : MonoBehaviour
{
    [Header("UIの色")]
    [SerializeField] private Color BeforeColor;  // 押す前
    [SerializeField] private Color AfterColor;   // 押す後

    private void Start()
    {
        // 初期化
        //GetComponent<Image>().color = BeforeColor;
    }

    public void ShotPush()
    {
        // 押したら0.1秒色を変える
        StartCoroutine(PushCoroutine());
    }

    public void Push()
    {
        // 押したら色を変える
        GetComponent<Image>().color = AfterColor;
    }

    public void Release()
    {
        // 離したら色を戻す
        GetComponent<Image>().color = BeforeColor;
    }

    IEnumerator PushCoroutine()
    {
        // 押したら色を変える
        GetComponent<Image>().color = AfterColor;

        // 0.1秒待つ
        yield return new WaitForSeconds(0.1f);

        // 離したら色を戻す
        GetComponent<Image>().color = BeforeColor;
    }

    public void PushTextUI()
    {
        // 押したら色を変える
        GetComponent<Text>().color = AfterColor;
    }

    public void ReleaseTextUI()
    {
        // 離したら色を戻す
        GetComponent<Text>().color = BeforeColor;
    }

}
