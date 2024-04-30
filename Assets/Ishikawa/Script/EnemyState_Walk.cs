using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Walk : EnemyState
{
    Transform m_PlayerTrans;
    NavMeshAgent m_Agent;
    float m_fRandMove;
    Vector3 m_TargetPos;
    enum State
    {
        Rotate,
        Walk,
    }
    State m_NowState;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_Agent = gameObject.GetComponent<NavMeshAgent>();
        //m_fRandMove = m_EnemyCom.GetRandMove();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (m_fFrameTime <= 0.0f)
            return;

        base.Update();

        switch (m_NowState)
        {
            case State.Rotate:
                break;
            case State.Walk:
                break;
        }
        // �ړI�n������ɒ������玟�̈ړ�������߂�
        float distX = m_TargetPos.x - transform.position.x;
        float distZ = m_TargetPos.x - transform.position.z;
        float Distans = distX * distX + distZ * distZ;
        if (Distans <= 10.0f)
        {
            m_TargetPos = MoveRandomPos();
        }
           
        // �ڕW�n�_�̕���������

        // ������x�����������
        m_Agent.SetDestination(m_TargetPos);

       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_TargetPos = MoveRandomPos();
    }

    Vector3 MoveRandomPos()
    {
        Vector3 pos = new Vector3(
            Random.Range(-m_fRandMove, m_fRandMove), transform.position.y, Random.Range(-m_fRandMove, m_fRandMove));
        return pos;
    }
}
