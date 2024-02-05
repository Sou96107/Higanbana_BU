using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    protected bool m_IsFinish;  // Ç±ÇÃèÛë‘ÇèIóπÇ∑ÇÈÇ©
    bool GetSetFinish { get { return m_IsFinish; }set { m_IsFinish = value; } }

    protected Enemy m_EnemyCom;
    protected GameObject m_PlayerObj;
    protected float m_fFrame;
    protected NavMeshAgent m_Agent;
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
        if (m_Agent == null)
            m_Agent = gameObject.GetComponent<NavMeshAgent>();
    }
}
