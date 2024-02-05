using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField, Header("エネルギー回復時間")]
    float EnergyHealTime = 5.0f;
    [Header("エネルギー消費量")]
    [SerializeField] float Step = 0.1f;        // ステップ時の消費割合
    [SerializeField] float Roll = 0.1f;        // ロール時の消費割合

    GameObject player;    // プレイヤー情報格納
    [Range(0, 100)] public float energy = 100;      // 現在のエネルギー（0から100の範囲で設定）
    private float energy_ratio;                     // 現在のエネルギーを最大エネルギーで割った比率
    private Image image_Energygauge;                // エネルギーゲージのUIイメージ
    private float maxEnergy = 100;                  // プレイヤーの最大エネルギー
    [HideInInspector] public float SubDash = 1.0f;                   // ダッシュ時のエネルギー消費量
    [HideInInspector] public float SubStep = 0.1f;                   // ステップ時のエネルギー消費量
    [HideInInspector] public float SubRoll = 0.1f;                   // ロール時のエネルギー消費量
    [HideInInspector] public bool isStep = false;                    // ステップ中かどうか
    [HideInInspector] public bool isRoll = false;                    // ロール中かどうか

    void Start()
    {
        // プレイヤー情報を取得
        //player = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.Find("Player");

        // エネルギーゲージのUIイメージを取得
        image_Energygauge = gameObject.GetComponent<Image>();

        // エネルギー最大値の設定
        maxEnergy = EnergyHealTime * 60;

        // モーション時のエネルギー消費量を計算
        SubStep = maxEnergy * Step;
        SubRoll = maxEnergy * Roll;

        energy = maxEnergy;
    }

    void FixedUpdate()
    {
        //-エネルギー値更新
        //if (player.GetComponent<Player>().isDash) 
        //{// ダッシュ時
        //    if (energy > 0)
        //        energy -= SubDash;
        //}
        //else 
        if (!player)
            return;

        if (player.GetComponent<Player>().isStep)
        {// ステップ時
            if (!isStep)
            {
                if (energy > SubStep)
                {
                    isStep = true;
                    energy -= SubStep;
                }
            }
        }
        else if (player.GetComponent<Player>().isRoll)
        {// ロール時
            if (!isRoll)
            {
                if (energy > SubStep)
                {
                    isRoll = true;
                    energy -= SubRoll;
                }
            }
        }
        else
        {// 動いてないor通常移動→回復
            if (energy < maxEnergy)
                energy += SubDash;
        }



        //-UIの更新
        // 現在のエネルギーを最大エネルギーで割った比率を計算
        energy_ratio = energy / maxEnergy;
        // エネルギーゲージの表示を更新
        image_Energygauge.fillAmount = energy_ratio;
    }
}
