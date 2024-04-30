using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Attack : EnemyState
{

    // ����p�i�x���@�j
    private float _sightAngle;

    // ���E�̍ő勗��
    float _maxDistance = float.PositiveInfinity;

    float m_Time;  // �v���p
    float m_BulletInterval;
    float m_OmenTime;
    float m_PerformTime;
    int m_BulletMaxNum;
    int m_ShootNum;     // ������������
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
        Add,    // ���X�g�i�[
        Wait,   // �҂�
        Omen,  // �\����
        Shoot,  // �e������
        Kick,  // �R��
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

        // �ǂ�������
        Chase();

        if (IsFound)
            LookPlayer();

        // �v���C���[���W���X�g�������Ȃ��ēG�Ƃ̋������߂�������R��
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
                    // �A�j���[�V�����Đ�
                    if (IsFound)
                        AnimEnemy.SetBool("isADS", true);
                    IsOnce = true;
                    break;

                case AttackState.Omen:

                    //if (m_Time >= m_BulletInterval)
                    //{
                    //    //�����ŗ\�������o��
                    //    Sen.SetLineColor(m_Time - m_BulletInterval);
                    //    Sen.DrawLine();

                    //    if (m_Time - m_BulletInterval >= m_OmenTime)
                    //    {
                    //        if (IsFound/* && IsVisible()*/)
                    //        {
                    //            // ����
                    //            Sen.EnebleLine();
                    //            m_NowState = AttackState.Shoot;
                    //            m_Time = 0.0f;
                    //            break;
                    //        }

                    //    }
                    //}

                    //�����ŗ\�������o��
                    Sen.SetLineColor(m_Time);
                    Sen.DrawLine();

                    if (m_Time >= m_OmenTime)
                    {
                        if (IsFound/* && IsVisible()*/)
                        {
                            // ����
                            Sen.EnebleLine();
                            m_NowState = AttackState.Shoot;
                            m_Time = 0.0f;
                            break;
                        }

                    }

                    m_Time += Time.deltaTime;
                    break;
                case AttackState.Shoot:
                    //�\�����̕\��
                    //GetComponent<createSen>().DrawLine();

                    // �e�𔭎˂�����\����������
                    //GetComponent<createSen>().EnebleLine();

                    // ����炷
                    SoundManagerObj.GetComponent<SoundManager>().PlaySE("���C��");

                    // �G�t�F�N�g��\��
                    StartCoroutine(EnemyEffectDrawTime());

                    // �A�j���[�V�����Đ�
                    //AnimEnemy.SetTrigger("trShot");
                    AnimEnemy.SetBool("isADS", false);
                    AnimEnemy.Play("Shot", 0, 0.0f);

                    // �e������
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
                    // �\����������
                    Sen.EnebleLine();

                    // �L�b�N�A�j���[�V�����J�n
                    if (IsOnce)
                    {
                        Debug.Log("�R��");
                        m_Time = 0.0f;
                        //AnimEnemy.SetBool("isADS", false);
                        AnimEnemy.Play("Kick", 0, 0.0f);
                        IsOnce = false;
                    }

                    // �R��A�j���[�V�����I��
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



    // ���E���ɓ����Ă��邩
    bool IsVisible()
    {

        // �^�[�Q�b�g�܂ł̌����Ƌ����v�Z
        Vector3 targetDir = m_PlayerObj.transform.position - transform.position;
        float targetDistance = targetDir.magnitude;

        // cos(��/2)���v�Z
        float cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z
        float innerProduct = Vector3.Dot(transform.forward, targetDir.normalized);

        // ���E����
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    void Chase()
    {
        float Distance;

        if (m_EnemyCom.GetSetIsFound) // �����Ă���ꍇ
        {
            Distance = Vector3.Distance(m_PlayerObj.transform.position, transform.position);
            // ��苗������Ă�Ȃ�
            if (Distance >= m_EnemyCom.GetStopDis())
            {
                m_Agent.isStopped = false;
                m_Agent.SetDestination(m_PlayerObj.transform.position);

                // �ړ��A�j���[�V�����Đ�
                AnimEnemy.SetBool("isMove", true);
            }
            else
            {
                //// �\����������
                //Sen.EnebleLine();
                m_Agent.isStopped = true;

                // �ړ��A�j���[�V������~
                AnimEnemy.SetBool("isMove", false);
            }
            
        }
        else  // �������Ă���ꍇ
        {
            // �\����������
            Sen.EnebleLine();

            AnimEnemy.SetBool("isMove", true);
            m_Agent.isStopped = false;
            // ���������ꏊ�܂ł͌�����
            m_Agent.SetDestination(m_EnemyCom.GetLostPos());

            // �t�߂ɒ�������
            Distance = Vector3.Distance(m_EnemyCom.GetLostPos(), transform.position);

            if (Distance <= 1.5f)
            {
                // �ړ��A�j���[�V������~
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
        // �v���C���[�̕���������
        // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o
        Vector3 vector3 = m_PlayerObj.transform.position - this.transform.position;

        vector3.y = 0f;

        // ��]�l���擾
        Quaternion quaternion = Quaternion.LookRotation(vector3);

        transform.rotation = Quaternion.Slerp(this.transform.rotation, quaternion, 0.02f);
    }

    IEnumerator EnemyEffectDrawTime()
    {
        // �G�t�F�N�g�̕\���A��\��
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
