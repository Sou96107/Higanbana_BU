using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clear : MonoBehaviour
{
    [Header("回転の中心点")]
    [SerializeField] GameObject centerObj; 
    [Header("アニメーター")]
    [SerializeField] Animator anim;
    [Header("エフェクト開始時間")]
    [SerializeField] float EffectStartTime = 3.0f;
    [SerializeField] SoundManager SoundMan;


    [SerializeField] private float curTime;
    private bool isBack;
    private bool isOnce;

    private void Start()
    {
        curTime = 0.0f;
        isBack = false;
        isOnce = false;
        SoundMan = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        // モデルの移動
        transform.RotateAround(centerObj.transform.position, Vector3.up, -360 / 45 * Time.deltaTime);
        // 経過時間の更新
        curTime += Time.deltaTime;
        
        if (!isBack && curTime > EffectStartTime)
        {
            SoundMan.PlaySE("クリア");
            anim.SetTrigger("StartUI");
            isBack = true;
        }

        if (curTime > 5.5f)
        {
            if (!isOnce)
            {
                isOnce = true;
                FadeIn.instance.NextScene("Title");
            }
        }
    }
}
