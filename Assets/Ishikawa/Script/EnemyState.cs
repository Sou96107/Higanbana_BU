using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyState : MonoBehaviour
{
    protected bool m_IsFinish;  // この状態を終了するか
    bool GetSetFinish { get { return m_IsFinish; }set { m_IsFinish = value; } }

    protected Enemy m_EnemyCom;
    protected GameObject m_PlayerObj;
    protected float m_fFrame;
    protected NavMeshAgent m_Agent;
    protected float m_fFrameTime;
    [NonSerialized] public Animator AnimEnemy;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        m_EnemyCom = gameObject.GetComponent<Enemy>();
        m_PlayerObj = m_EnemyCom.GetPlayer();
        AnimEnemy = GetComponent<Enemy>().enemyModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        m_fFrame += Time.deltaTime;
    }

    virtual protected void OnEnable()
    {
        m_IsFinish = false;
        m_fFrame = 0.0f;
        m_fFrameTime = 1.0f;
        if (m_Agent == null)
            m_Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public void StartFrame()
    {
        m_fFrameTime = 1.0f;
        Debug.Log("エネミーフレーム：" + m_fFrameTime);
    }

    public void StopFrame()
    {
        m_fFrameTime = 0.0f;
        Debug.Log("エネミーフレーム：" + m_fFrameTime);
    }
}
