using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private GameObject player;

    [SerializeField, Header("HPBarの後ろのImage")]
    private Image image_Backgauge;

    // プレイヤーの最大HP
    public float maxHP = 100;

    // 現在のHP（0から100の範囲で設定）
    [Range(0, 100)] public float hp = 100;

    // HSL色空間のパラメータ
    [Range(0, 1)] public float color_h, color_s, color_l;

    // HPゲージのUIイメージ
    private Image image_HPgauge;

    // 現在のHPを最大HPで割った比率
    private float hp_ratio;

    private float back_ratio;
    private float speed = 0.0005f;
    private float acc = 0.0f;

    // プレイヤーのスクリプト
    private Player playerCS;

    // スクリプトの開始時に呼び出されるメソッド
    void Start()
    {
        // プレイヤーのスクリプト取得
        playerCS = player.GetComponent<Player>();

        maxHP = playerCS.GetHP();

        // HPゲージのUIイメージを取得
        image_HPgauge = gameObject.GetComponent<Image>();

        back_ratio = 1.0f;

        // 初期の色のS（彩度）とL（明度）を設定
        color_s = 1.0f;
        color_l = 0.5f;
    }

    // HSL色空間からRGB色を計算するメソッド
    Color HSLtoRGB(float H, float S, float L)
    {
        Color col = new Color(1f, 1f, 1f, 1f);

        // 色相に基づいてRGB色を計算
        float max = L + S * (1 - Mathf.Abs(2f * L - 1f)) * 0.5f;
        float min = L - S * (1 - Mathf.Abs(2f * L - 1f)) * 0.5f;

        int i = (int)Mathf.Floor(6.0f * H);

        switch (i)
        {
            case 0:
                col.r = max;
                col.g = min + (max - min) * 6.0f * H;
                col.b = min;
                break;
            case 1:
                col.r = min + (max - min) * (2.0f - 6.0f * H);
                col.g = max;
                col.b = min;
                break;
            case 2:
                col.r = min;
                col.g = max;
                col.b = min + (max - min) * (6.0f * H - 2.0f);
                break;
            case 3:
                col.r = min;
                col.g = min + (max - min) * (4.0f - 6.0f * H);
                col.b = max;
                break;
            case 4:
                col.r = min + (max - min) * (6.0f * H - 4.0f);
                col.g = min;
                col.b = max;
                break;
            case 5:
                col.r = max;
                col.g = min;
                col.b = min + (max - min) * (6.0f - 6.0f * H);
                break;
            default:
                col.r = max;
                col.g = max;
                col.b = max;
                break;
        }
        return col;
    }

    void Update()
    {
        // HPを減少させる（テスト用）
        //hp -= 0.1f;
        // プレイヤーのHPを取得
        hp = playerCS.GetHP();
        // 現在のHPの割合を計算
        hp_ratio = hp / maxHP;

        // HPゲージのUIを更新
        image_HPgauge.fillAmount = hp_ratio;

        if (hp_ratio <= back_ratio)
        {
            back_ratio -= speed + acc;
            image_Backgauge.fillAmount = back_ratio;
            acc += speed * Time.deltaTime;
        }
        else
            acc = 0.0f;

        // 色相をHPに応じて変化させる
        color_h = Mathf.Lerp(0f, 0.4f, hp_ratio);

        // HPゲージの色を設定
        image_HPgauge.color = HSLtoRGB(color_h, color_s, color_l);
    }
}