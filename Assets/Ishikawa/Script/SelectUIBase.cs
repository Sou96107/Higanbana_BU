using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectUIBase : MonoBehaviour
{
    [SerializeField,Header("何倍にするか")]
    float MultiplyValueScale = 1.5f;
    [SerializeField]
    float SwayTime = 1.5f;

    // 選択前
    Vector3 m_BaseScale;
    Color m_BaceColor;
    // 選択後
    Vector3 m_BigScale;
    Color m_ChangeColor;

    bool m_Execute; // 選択されているか
    bool m_IsTrans; // 透明か
    bool m_IsOnce;
    float m_time;
    Image image;
    KirariImage kirari;
    // Start is called before the first frame update
    protected virtual void Start()
    {

        Debug.Log("あああ");
        image = GetComponent<Image>();
        kirari = transform.GetComponentInChildren<KirariImage>();
        m_BaseScale = transform.localScale;
        m_BaceColor = image.color;
        m_BigScale = transform.localScale * MultiplyValueScale;
        Debug.Log(m_BigScale);
        Debug.Log(m_BaseScale);
        Debug.Log(transform.localScale);
        m_ChangeColor = new Color(m_BaceColor.r, m_BaceColor.g, m_BaceColor.b, 0.1f);
        m_Execute = false;
        m_IsTrans = false;
        m_IsOnce = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!m_Execute)
        {
            return;
        }

        if (!m_IsTrans)
        {
            image.color = Vector4.Lerp(m_BaceColor, m_ChangeColor, m_time);
            if (m_time >= 1.0f)
            {
                m_time = 0.0f;
                m_IsTrans = true;
                return;
            }
        }
        else
        {
            image.color = Vector4.Lerp(m_ChangeColor, m_BaceColor, m_time);
            if (m_time >= 1.0f)
            {
                m_time = 0.0f;
                m_IsTrans = false;
                return;
            }

            if (m_time >= 0.5f)
                kirari.SetOn();

        }

        m_time += Time.unscaledDeltaTime;
    }

    public void StartChangeScaleTitle()
    {
        m_Execute = true;
        transform.DOScale(m_BigScale, 0.15f);
        image.color = m_BaceColor;
    }

    public void StopChangeScaleTitle()
    {
        m_Execute = false;
        transform.DOScale(m_BaseScale, 0.15f);
        image.color = m_BaceColor;
        m_IsTrans = false;
        kirari.SetReset();

    }

    public void StartChangeScalePause()
    {
        m_Execute = true;
        image.color = m_BaceColor;
    }

    public void StopChangeScalePause()
    {
        m_Execute = false;
        image.color = m_BaceColor;
        m_IsTrans = false;
        kirari.SetReset();
    }

    public virtual void Action()
    {

    }
}
