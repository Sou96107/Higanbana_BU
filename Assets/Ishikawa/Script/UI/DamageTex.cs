using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTex : MonoBehaviour
{
    [SerializeField,Header("拡大比率")] float ratio = 2.0f;
    [SerializeField, Header("拡縮スピード")] float speed = 5.0f;
    [SerializeField, Header("フェードスピード")] float feedspeed = 3.0f;
    Text t;
    int num;
    float rate;
    Vector3 BaseScale;
    Vector3 BigScale;
    Color BaseColor;
    Color OutColor;
    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        rate = 0.0f;
        BaseScale = transform.localScale;
        BigScale = BaseScale * ratio;
        BaseColor = t.color;
        OutColor = new Vector4(BaseColor.r, BaseColor.g, BaseColor.b, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (num)
        {
            case 0: // 大きくしていく
                transform.localScale = Vector3.Lerp(BaseScale, BigScale, rate);

                // rateが1以上になった
                if (rate >= 1.0f)
                {
                    rate = 0.0f;
                    num++;
                    break;
                }

                rate += Time.deltaTime * speed;
                break;
            case 1: // 標準サイズに戻っていく
                transform.localScale = Vector3.Lerp(BigScale, BaseScale, rate);

                // rateが1以上になった
                if (rate >= 1.0f)
                {
                    rate = 0.0f;
                    num++;
                    break;
                }

                rate += Time.deltaTime * speed;
                break;
            case 2: // フェードアウト
                t.color = Vector4.Lerp(BaseColor, OutColor, rate);
                // rateが1以上になった
                if (rate >= 1.0f)
                {
                    rate = 0.0f;
                    num++;
                    
                    Destroy(transform.parent.parent.gameObject);
                    break;
                }

                rate += Time.deltaTime * feedspeed;
                break;
            default:
                break;
        }
    }

    public void SetDamage(float damage)
    {
        t = GetComponent<Text>();
        t.text = "" + damage;
    }
}
