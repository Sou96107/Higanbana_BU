using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Attack : EnemyState
{

    // 視野角（度数法）
    private float _sightAngle;

    // 視界の最大距離
    float _maxDistance = float.PositiveInfinity;

    float m_Time;  // 計測用
    float m_BulletInterval;
    float m_OmenTime;
    float m_PerformTime;
    int m_BulletMaxNum;
    int m_ShootNum;     // 何発撃ったか
    bool m_isPerformance;
    GameObject MuzzleObj;
    createSen Sen;
    GameObject SoundManagerObj;
    GameObject muzzleFlash;
    GameObject KickCollider;
    bool IsOnce;
    AnimatorStateInfo stateInfo;
    EnemyShotManager enemShotMG;
    enum AttackState
    {
        Add,    // リスト格納
        Wait,   // 待ち
        Omen,  // 予測線
        Shoot,  // 弾を撃つ
        Kick,  // 蹴る
    }

    AttackState m_NowState;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _sightAngle = 20.0f;
        _maxDistance = 15.0f;
        m_BulletInterval = m_EnemyCom.GetBulletInterval();
        m_OmenTime = m_EnemyCom.GetOmenTime();
        m_BulletMaxNum = m_EnemyCom.GetBulletMaxNum();
        MuzzleObj = transform.Find("Gun/muzzle").gameObject;
        Sen = GetComponent<createSen>();
        SoundManagerObj = GameObject.Find("SoundManager");
        muzzleFlash = GetComponent<Enemy>().MuzzleFlashObj;
        enemShotMG = GameObject.Find("EnemyShotManager").GetComponent<EnemyShotManager>();
        m_isPerformance = false;
        m_PerformTime = 0.0f;
        IsOnce = true;
        KickCollider = transform.Find("KickCollision").gameObject;
        m_PlayerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (m_PlayerObj == null || m_PlayerObj.GetComponent<Player>().hp <= 0)
            return;

        if (AnimEnemy.GetBool("isDeath"))
            return;

        if (m_fFrameTime <= 0.0f)
            return;

        base.Update();

        bool IsFound = m_EnemyCom.GetSetIsFound;

        stateInfo = AnimEnemy.GetCurrentAnimatorStateInfo(0);

        // 追いかける
        Chase();

        if (IsFound)
            LookPlayer();

        // プレイヤーがジャスト回避じゃなくて敵との距離が近かったら蹴る
        float Dis = Vector3.Distance(m_PlayerObj.transform.position, transform.position);
        if (Dis <= 2.3f && !m_PlayerObj.GetComponent<Player>().IsJust() && IsOnce)
        {
            m_NowState = AttackState.Kick;
        }

        if (!m_isPerformance)
        {
            switch (m_NowState)
            {
                case AttackState.Add:
                    enemShotMG.AddEnemy(transform.parent.gameObject);
                    m_NowState = AttackState.Wait;
                    break;

                case AttackState.Wait:
                    // アニメーション再生
                    if (IsFound)
                        AnimEnemy.SetBool("isADS", true);
                    IsOnce = true;
                    break;

                case AttackState.Omen:

                    //if (m_Time >= m_BulletInterval)
                    //{
                    //    //ここで予測線を出す
                    //    Sen.SetLineColor(m_Time - m_BulletInterval);
                    //    Sen.DrawLine();

                    //    if (m_Time - m_BulletInterval >= m_OmenTime)
                    //    {
                    //        if (IsFound/* && IsVisible()*/)
                    //        {
                    //            // 消す
                    //            Sen.EnebleLine();
                    //            m_NowState = AttackState.Shoot;
                    //            m_Time = 0.0f;
                    //            break;
                    //        }

                    //    }
                    //}

                    //ここで予測線を出す
                    Sen.SetLineColor(m_Time);
                    Sen.DrawLine();

                    if (m_Time >= m_OmenTime)
                    {
                        if (IsFound/* && IsVisible()*/)
                        {
                            // 消す
                            Sen.EnebleLine();
                            m_NowState = AttackState.Shoot;
                            m_Time = 0.0f;
                            break;
                        }

                    }

                    m_Time += Time.deltaTime;
                    break;
                case AttackState.Shoot:
                    //予測線の表示
                    //GetComponent<createSen>().DrawLine();

                    // 弾を発射したら予測線を消す
                    //GetComponent<createSen>().EnebleLine();

                    // 音を鳴らす
                    SoundManagerObj.GetComponent<SoundManager>().PlaySE("発砲音");

                    // エフェクトを表示
                    StartCoroutine(EnemyEffectDrawTime());

                    // アニメーション再生
                    //AnimEnemy.SetTrigger("trShot");
                    AnimEnemy.SetBool("isADS", false);
                    AnimEnemy.Play("Shot", 0, 0.0f);

                    // 弾を撃つ
                    GameObject Bullet = Instantiate(m_EnemyCom.GetBullet(), MuzzleObj.transform.position, Quaternion.identity);
                    Bullet.transform.forward = GameObject.Find("TargetPos").transform.position - MuzzleObj.transform.position;
                    Bullet.GetComponent<EnemyBullet>().SetBullet(
                        Bullet.transform.forward.x, 0.0f, Bullet.transform.forward.z);
                    Bullet.GetComponent<createSen>().SetPos(GetComponent<createSen>().player, Bullet.transform);
                    Bullet.GetComponent<EnemyBullet>().SetPower(m_EnemyCom.GetPower());
                    Bullet.GetComponent<EnemyBullet>().SetEnemyName(m_EnemyCom.GetName());
                    m_ShootNum++;

                    m_NowState = AttackState.Wait;
                    break;
                case AttackState.Kick:
                    // 予測線を消す
                    Sen.EnebleLine();

                    // キックアニメーション開始
                    if (IsOnce)
                    {
                        Debug.Log("蹴り");
                        m_Time = 0.0f;
                        //AnimEnemy.SetBool("isADS", false);
                        AnimEnemy.Play("Kick", 0, 0.0f);
                        IsOnce = false;
                    }

                    // 蹴りアニメーション終了
                    if (m_Time >= 1 && 1.0f <= stateInfo.normalizedTime)
                    {
                        IsOnce = true;
                        m_Time = 0.0f;
                        m_NowState = AttackState.Wait;
                        break;
                    }
                    m_Time++;
                    break;
            }
        }
        else
        {
            m_PerformTime += Time.deltaTime;
            if(m_PerformTime >= 0.15f)
            {
                m_NowState = AttackState.Shoot;
                m_PerformTime = 0.0f;
                m_isPerformance = false;
            }
        }

        if (!IsFound)
        {
            Sen.EnebleLine();
            AnimEnemy.SetBool("isADS", false);
            m_Time = 0.0f;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //m_Agent.isStopped = true;

        m_NowState = AttackState.Add;
        m_Time = 0.0f;
        m_ShootNum = 0;
    }



    // 視界内に入っているか
    bool IsVisible()
    {

        // ターゲットまでの向きと距離計算
        Vector3 targetDir = m_PlayerObj.transform.position - transform.position;
        float targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        float cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        float innerProduct = Vector3.Dot(transform.forward, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    void Chase()
    {
        float Distance;

        if (m_EnemyCom.GetSetIsFound) // 見つけている場合
        {
            Distance = Vector3.Distance(m_PlayerObj.transform.position, transform.position);
            // 一定距離離れてるなら
            if (Distance >= m_EnemyCom.GetStopDis())
            {
                m_Agent.isStopped = false;
                m_Agent.SetDestination(m_PlayerObj.transform.position);

                // 移動アニメーション再生
                AnimEnemy.SetBool("isMove", true);
            }
            else
            {
                //// 予測線を消す
                //Sen.EnebleLine();
                m_Agent.isStopped = true;

                // 移動アニメーション停止
                AnimEnemy.SetBool("isMove", false);
            }
            
        }
        else  // 見失っている場合
        {
            // 予測線を消す
            Sen.EnebleLine();

            AnimEnemy.SetBool("isMove", true);
            m_Agent.isStopped = false;
            // 見失った場所までは向かう
            m_Agent.SetDestination(m_EnemyCom.GetLostPos());

            // 付近に着いたら
            Distance = Vector3.Distance(m_EnemyCom.GetLostPos(), transform.position);

            if (Distance <= 1.5f)
            {
                // 移動アニメーション停止
                AnimEnemy.SetBool("isMove", false);
                m_Agent.isStopped = true;
                m_EnemyCom.ChangeState(0);
                enemShotMG.SubEnemy(transform.parent.gameObject);
                return;
            }
                
        }
    }

    void LookPlayer()
    {
        // プレイヤーの方向を向く
        // 対象物と自分自身の座標からベクトルを算出
        Vector3 vector3 = m_PlayerObj.transform.position - this.transform.position;

        vector3.y = 0f;

        // 回転値を取得
        Quaternion quaternion = Quaternion.LookRotation(vector3);

        transform.rotation = Quaternion.Slerp(this.transform.rotation, quaternion, 0.02f);
    }

    IEnumerator EnemyEffectDrawTime()
    {
        // エフェクトの表示、非表示
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.SetActive(false);
    }

    public void PerformanceShot()
    {
        //m_NowState = AttackState.Shoot;
        m_isPerformance = true;
        m_Time = 0.0f;
    }

    public bool ChangeState()
    {
        if (m_NowState != AttackState.Omen && m_NowState != AttackState.Shoot && m_NowState != AttackState.Kick)
        {
            m_NowState = AttackState.Omen;
            return true;
        }
        return false;
    }
}
