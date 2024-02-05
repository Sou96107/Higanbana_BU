using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMove : MonoBehaviour
{
    [SerializeField, Header("回転の中心点")]
    private GameObject centerObj;
    [SerializeField, Header("フェードイメージ")]
    private CanvasGroup fade;
    [SerializeField, Header("フェード開始時間")]
    private float fadeStartTime = 1.0f;
    [SerializeField, Header("フェード速度")]
    private float fadeTime = 1.0f;
    [SerializeField, Header("クリアUIイメージ①")]
    private RectTransform clearUI_RT;
    [SerializeField, Header("クリアUIイメージ②")]
    private CanvasGroup clearUI_CG;
    [SerializeField, Header("クリアUIサイズ(最小)")]
    private float clearUI_minSize;
    [SerializeField, Header("クリアUIサイズ(普通)")]
    private float clearUI_normalSize;

    private float curFadeTime; // フェードまでの現在時間
    private Vector3 currentSize; // 
    private float addSize;
    private bool isAdd;
    private bool isCalc;
    private bool isOnce;

    // Start is called before the first frame update
    void Start()
    {
        curFadeTime = 0.0f;
        currentSize = clearUI_RT.localScale;
        addSize = clearUI_RT.localScale.x;
        isAdd = false;
        isCalc = true;
        isOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        // モデルの移動
        transform.RotateAround(centerObj.transform.position, Vector3.up, -360 / 45 * Time.deltaTime);
        // 経過時間の更新
        curFadeTime += Time.deltaTime;

        if(curFadeTime > fadeStartTime)
        {
            // アルファ値の更新
            fade.alpha += fadeTime * Time.deltaTime;
            clearUI_CG.alpha += fadeTime * Time.deltaTime;

            if (isCalc)
            {
                if (!isAdd)
                {
                    if (addSize > clearUI_minSize)
                        addSize += (clearUI_minSize - currentSize.x) * 4 * Time.deltaTime;
                    if (addSize <= clearUI_minSize)
                    {
                        currentSize = clearUI_RT.localScale;
                        isAdd = true;
                    }
                }
                else
                {
                    if (addSize < clearUI_normalSize)
                        addSize += (clearUI_normalSize - currentSize.x) * 4 * Time.deltaTime;
                    if (addSize >= clearUI_normalSize)
                        isCalc = false;
                }
                clearUI_RT.localScale = new Vector3(addSize, addSize, addSize);
            }
        }
        if (curFadeTime > 5.5f)
        {
            if (!isOnce)
            {
                isOnce = true;
                FadeIn.instance.NextScene("Title");
            }
        }
    }
}
