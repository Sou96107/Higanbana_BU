using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : EnemyState
{
    float m_CountTime;
    int m_nTime;  // 追いかける時間

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
        if (m_EnemyCom.GetSetIsFound)　// 見つけている場合
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(m_PlayerObj.transform.position);
            // 追いかけて撃つように
            if (m_CountTime >= m_nTime)
            {
                //m_Agent.isStopped = true;
                m_EnemyCom.ChangeState(2);
                return;
            }
            m_CountTime += Time.deltaTime;
        }
        else  // 見失っている場合
        {
            // 見失った場所までは向かう
            m_Agent.SetDestination(m_EnemyCom.GetLostPos());

            // 付近に着いたら
            Distance = Vector3.Distance(m_EnemyCom.GetLostPos(), transform.position);
            if (Distance <= 0.5f)
            {
                m_Agent.isStopped = true;
                m_EnemyCom.ChangeState(0);
                return;
            }
        }
        

        //// プレイヤーと一定距離離れたら追うのをやめる
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
        // 何秒追うかランダムで
        m_nTime = Random.Range(1, 4); //1～3秒
    }
}
