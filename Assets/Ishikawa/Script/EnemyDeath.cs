using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath: MonoBehaviour
{
    public GameObject Enemy;

    void Death()
    {
        Enemy.GetComponent<Enemy>().Death();
    }

    void DawnSE()
    {
        GameObject SoundManagerObj = GameObject.Find("SoundManager");
        SoundManagerObj.GetComponent<SoundManager>().PlaySE("“|‚ê‚é");
    }
}
