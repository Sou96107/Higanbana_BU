using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : EnemyState
{
    float m_CountTime;
    int m_nTime;  // �ǂ������鎞��

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float Distance;
        if (m_EnemyCom.GetSetIsFound)�@// �����Ă���ꍇ
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(m_PlayerObj.transform.position);
            // �ǂ������Č��悤��
            if (m_CountTime >= m_nTime)
            {
                //m_Agent.isStopped = true;
                m_EnemyCom.ChangeState(2);
                return;
            }
            m_CountTime += Time.deltaTime;
        }
        else  // �������Ă���ꍇ
        {
            // ���������ꏊ�܂ł͌�����
            m_Agent.SetDestination(m_EnemyCom.GetLostPos());

            // �t�߂ɒ�������
            Distance = Vector3.Distance(m_EnemyCom.GetLostPos(), transform.position);
            if (Distance <= 0.5f)
            {
                m_Agent.isStopped = true;
                m_EnemyCom.ChangeState(0);
                return;
            }
        }
        

        //// �v���C���[�ƈ�苗�����ꂽ��ǂ��̂���߂�
        //Distance = Vector3.Distance(m_PlayerObj.transform.position, transform.position);
        //if (Distance >= m_EnemyCom.GetStopChaseDis())
        //{
        //    m_Agent.isStopped = true;
        //    m_EnemyCom.ChangeState(0);
        //    return;

        //}
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        m_Agent.isStopped = false;
        m_CountTime = 0.0f;
        // ���b�ǂ��������_����
        m_nTime = Random.Range(1, 4); //1�`3�b
    }
}
