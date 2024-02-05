using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField, Header("敵")]
    private GameObject enemy;

    [SerializeField, Header("HPBarの後ろのImage")]
    private Image image_Backgauge;

    // プレイヤーの最大HP
    float maxHP = 100;

    // 現在のHP（0から100の範囲で設定）
    float hp = 100;

    // HSL色空間のパラメータ
    [Range(0, 1)] public float color_h, color_s, color_l;

    // HPゲージのUIイメージ
    private Image image_HPgauge;

    // 現在のHPを最大HPで割った比率
    private float hp_ratio;

    private float back_ratio;
    private float speed = 0.05f;
    private float acc = 0.0f;

    // プレイヤーのスクリプト
    private Enemy EnemyCS;
    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのスクリプト取得
        EnemyCS = enemy.GetComponent<Enemy>();
        maxHP = EnemyCS.GetMaxHp();
        Debug.Log(maxHP);
        // HPゲージのUIイメージを取得
        image_HPgauge = gameObject.GetComponent<Image>();

        back_ratio = 1.0f;

        // 初期の色のS（彩度）とL（明度）を設定
        color_s = 1.0f;
        color_l = 0.5f;

    }

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
                //col.r = max;
                //col.g = min + (max - min) * 6.0f * H;
                //col.b = min;
                col.r = max;
                col.g = min;
                col.b = min + (max - min) * 6.0f * H;
                break;
            case 1:
                //col.r = min + (max - min) * (2.0f - 6.0f * H);
                //col.g = max;
                //col.b = min;
                col.r = min + (max - min) * (2.0f - 6.0f * H);
                col.g = min;
                col.b = max;
                break;
            case 2:
                //col.r = min;
                //col.g = max;
                //col.b = min + (max - min) * (6.0f * H - 2.0f);
                col.r = min;
                col.g = min + (max - min) * (6.0f * H - 2.0f);
                col.b = max;
                break;
            case 3:
                //col.r = min;
                //col.g = min + (max - min) * (4.0f - 6.0f * H);
                //col.b = max;
                col.r = min;
                col.g = max;
                col.b = min + (max - min) * (4.0f - 6.0f * H); ;
                break;
            case 4:
                //col.r = min + (max - min) * (6.0f * H - 4.0f);
                //col.g = min;
                //col.b = max;
                col.r = min + (max - min) * (6.0f * H - 4.0f);
                col.g = max;
                col.b = min;
                break;
            case 5:
                //col.r = max;
                //col.g = min;
                //col.b = min + (max - min) * (6.0f - 6.0f * H);
                col.r = max;
                col.g = min + (max - min) * (6.0f - 6.0f * H);
                col.b = min;
                break;
            default:
                col.r = max;
                col.g = max;
                col.b = max;
                break;
        }
        return col;
    }

    // Update is called once per frame
    void Update()
    {

        // 敵のHPを取得
        hp = EnemyCS.GetHp();

        // 現在のHPの割合を計算
        hp_ratio = hp / maxHP;

        // HPゲージのUIを更新
        image_HPgauge.fillAmount = hp_ratio;

        // Hpが減った
        if (hp_ratio <= back_ratio)
        {
            // 少しずつ加速しながらゲージを減らす
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
