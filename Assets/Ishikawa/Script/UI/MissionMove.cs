using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionMove : MonoBehaviour
{
    [SerializeField] float Speed = 1.0f;
    [SerializeField] RectTransform AfterTrans;
    RectTransform m_trans;
    Vector3 m_OriginScale;
    Vector3 m_AfterScale;
    Vector2 m_OriginPos;
    Vector2 m_AfterPos;
    float m_InterpolationScale; // スケール補間値
    float m_InterpolationPos; // ポジション補間値
    bool m_Finish;
    // Start is called before the first frame update
    void Start()
    {
        m_trans = GetComponent<RectTransform>();
        m_OriginScale = m_trans.localScale;
        m_OriginPos = m_trans.anchoredPosition;
        m_AfterScale = AfterTrans.localScale;
        m_AfterPos = AfterTrans.anchoredPosition;
        m_InterpolationScale = 0.0f;
        m_InterpolationPos = 0.0f;
        m_Finish = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(m_OriginScale, m_AfterScale, m_InterpolationScale);
        m_trans.anchoredPosition = Vector2.Lerp(m_OriginPos, m_AfterPos, m_InterpolationPos);

        

        // 移動終了
        if (m_InterpolationPos >= 1.0f)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 3.0f);
            m_Finish = true;
        }
        else
            //transform.Rotate(0.0f, 0.0f, 3.0f);


        m_InterpolationScale += Speed * Time.deltaTime;
        m_InterpolationPos += Speed * Time.deltaTime;
    }

    public bool GetFinish()
    {
        return m_Finish;
    }
}
