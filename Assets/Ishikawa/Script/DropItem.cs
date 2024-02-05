using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField, Header("何秒後消えるか")] float DestTime = 30.0f;
    protected bool m_IsContact;
    protected GameObject m_Player;
    public GameObject m_soundmanager;
    float m_LimitHigh;
    float m_LimitLow;
    float m_Speed;
    float m_CountTime;
    bool m_UpFlg;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_IsContact = false;
        m_Player = GameObject.Find("Player");
        m_LimitHigh = transform.position.y + 0.1f;
        m_LimitLow = transform.position.y - 0.1f;
        m_Speed = 0.1f;
        m_CountTime = 0.0f;
        m_UpFlg = false;
        m_soundmanager = GameObject.Find("SoundManager");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 上下に動かす
        if (m_UpFlg)
        {
            transform.position += new Vector3(0.0f,m_Speed * Time.deltaTime,0.0f);
        }
        else
        {
            transform.position -= new Vector3(0.0f, m_Speed * Time.deltaTime, 0.0f);
        }

        if (transform.position.y >= m_LimitHigh)
            m_UpFlg = false;
        else if (transform.position.y <= m_LimitLow)
            m_UpFlg = true;

        // 指定秒数経ったら消える
        if (m_CountTime >= DestTime)
        {
            Destroy(gameObject);
            return;
        }

        m_CountTime += Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // 接触したのがプレイヤーなら
        if (other.transform.tag.Contains("Player"))
        {
            m_IsContact = true;
        }

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // 離れたのがプレイヤーなら
        if (other.transform.tag.Contains("Player"))
        {
            m_IsContact = false;
        }
    }
}
