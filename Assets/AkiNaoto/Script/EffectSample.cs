/*
 ※注意！完成したエフェクトの素材をUnityに入れる際には階層構造に注意しましょう。
 以下のサイトの一番下に説明が載ってますが、とりあえずはAssets/Resourcesの中に
 エフェクトファイルを、テクスチャはさらにその中のTextureファイルに中へ入れればOK
 https://note.com/hirokichi0623/n/n9beb5b093033
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class EffectSample : MonoBehaviour
{
    public bool bPlayEffect;//試しに入れたエフェクト使用可否判定変数

    // Start is called before the first frame update
    void Start()
    {
        if(bPlayEffect)
        {
            PlayEffect("Laser01");

            PlayEffect("Simple_Ring_Shape2");
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===================================================================================
    //エフェクト再生関数
    //第一引数：再生したいエフェクトの名前
    //===================================================================================
    public void PlayEffect(string EffectName)
    {
        // エフェクトを取得する。
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(EffectName);
        // transformの位置でエフェクトを再生する
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, transform.position);
        // transformの回転を設定する。
        handle.SetRotation(transform.rotation);
    }

    public void PlayEffect(string EffectName, Vector3 pos)
    {
        // エフェクトを取得する。
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(EffectName);
        // transformの位置でエフェクトを再生する
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, pos);
        // transformの回転を設定する。
        handle.SetRotation(transform.rotation);
    }
}
