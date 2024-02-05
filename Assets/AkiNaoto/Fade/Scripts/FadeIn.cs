/*
フェードインやり方 
１．ゲームシーンに空のゲームオブジェクトを作成し、そこにこのスクリプトを追加
２．ヒエラルキーにFadeCanvasを追加し、インスペクタービューのマスクテクスチャに好きなマスク画像を選択
３．空のゲームオブジェクトを取得、参照するなどしてこのスクリプトを呼び出す

 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    [SerializeField] FadeT fade;
    [SerializeField] float FadeTime_Inst;//フェイドにかかる時間

    public string NextSceneName;    //次のシーン名
    public static float FadeTime;   //フェードにかかる時間
    public GameObject Text;

    public static FadeIn instance = null;

    void Awake()
    {
        FadeTime = FadeTime_Inst;
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void NextScene()
    {
        Action act = () => SceneManager.LoadScene(NextSceneName);
        float time = FadeTime;

        //時間経過してからにシーン移動する
        fade.FadeIn(time, act);
    }

    public void NextScene(string SceneName)
    {

        NextSceneName = SceneName;
        Action act = () => SceneManager.LoadScene(NextSceneName);
        float time = FadeTime;

        //時間経過してからにシーン移動する
        fade.FadeIn(time, act);
    }

    public void FadeOnly()
    {
        float time = FadeTime;
        Action act = () => Text.GetComponent<Fadetext>().Fadestart();

        //時間経過してからにシーン移動する
        fade.FadeIn(time, act);
    }
}
