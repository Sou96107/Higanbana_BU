using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_None : EnemyState
{
    Animator AnimEnemy;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_Agent.isStopped = true;
    }

}
