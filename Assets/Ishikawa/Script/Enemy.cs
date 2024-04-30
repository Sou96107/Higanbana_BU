using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("�̗�")]  float Hp = 3.0f;
    [SerializeField, Header("���s�X�s�[�h")] float Speed = 5.0f;
    [SerializeField, Header("�\�������o���b��")]  float OmenTime = 1.0f;
    //[SerializeField, Header("�p�j���̈ړ���")]  float RandMove = 5.0f;
    [SerializeField, Header("�v���C���[�ɋ߂Â��̂���߂鋗��")] float StopDistance = 1.0f;
    //[SerializeField, Header("�ǂ��̂���߂鋗��")]  float StopChaseDis = 20.0f;
    [SerializeField, Header("�U����")] int Power = 1;
    [SerializeField, Header("�R��̈З�")] float KickPower = 5.0f;
    [SerializeField, Header("�e�ۃI�u�W�F�N�g")] public GameObject BulletObj;
    [SerializeField, Header("�e���˃C���^�[�o��(�b��)")] float BulletInterval = 0.5f;
    [SerializeField, Header("�e�ې�")] int BulletMaxNum = 1;
    [SerializeField, Header("���Ƃ��A�C�e��")] GameObject DropItemObj;
    [SerializeField, Range(0, 100), Header("���Ƃ��m��(��)")] int DropRate = 25;
    [SerializeField, Header("�m�Y���t���b�V���I�u�W�F")] public GameObject MuzzleFlashObj;
    [SerializeField,Header("�_���[�W�\���L�����o�X")] GameObject Damagecanvas;
    [SerializeField, Header("�Ə��\���L�����o�X")] GameObject Aimcanvas;
    [SerializeField, Header("���f���I�u�W�F�N�g")] public GameObject enemyModel;
    [SerializeField, Header("�m�Y���I�u�W�F�N�g")] public Transform MuzzleTrs;
    [SerializeField, Header("�J�b�g�C���I�u�W�F�N�g")] public GameObject cutIn;
    [SerializeField, Header("���b�V��")] SkinnedMeshRenderer mesh;
    EnemyState[] m_StateCom;  // StateComponent�̔z��
    [SerializeField] EnemyState m_NowState;  // ���݂�State

    [System.NonSerialized] public GameObject m_PlayerObj;
    float m_Time;
    float m_ChaseTime;
    RaycastHit hit;
    int m_nStateNum;
    float m_maxHP;
    GameObject canvas;
    [System.NonSerialized] public bool m_IsFound;
    public bool GetSetIsFound { get { return m_IsFound; } set { m_IsFound = value; } }

    [System.NonSerialized] public Vector3 m_LostPos;
    public Vector3 GetSetLostPos { get { return m_LostPos; } set { m_LostPos = value; } }
    [System.NonSerialized] public bool IsLockOn;
    private bool IsJust;
    private bool IsOnce;
    private float m_frameTime;

    private void Awake()
    {
        m_maxHP = Hp;
    }
    // Start is called before the first frame update
    void Start()
    {
        // �ϐ��ɒl���
        m_StateCom = new EnemyState[3];
        m_PlayerObj = GameObject.Find("Player");
        GetComponent<createSen>().player = GameObject.Find("TargetPos").transform;
        m_Time = 0.0f;
        m_IsFound = false;
        m_nStateNum = 0;
        GetComponent<NavMeshAgent>().speed = Speed;
        IsLockOn = false;
        IsJust = false;
        IsOnce = false;
        m_frameTime = 1.0f;
        FrameManager.Add(gameObject, "Enemy");

        //m_NowState = gameObject.AddComponent<EnemyState_None>(); // �ŏ���NoneState
        // �����Ŏg��State�R���|�[�l���g�ǉ�
        m_StateCom[0] = gameObject.AddComponent<EnemyState_None>(); // �������Ȃ�
        m_StateCom[1] = gameObject.AddComponent<EnemyState_Chase>(); // �ǐ�
        m_StateCom[2] = gameObject.AddComponent<EnemyState_Attack>(); // �U�� 
        m_NowState = m_StateCom[0];
        Debug.Log(m_NowState);
        
        // �ŏ���NoneState����enable��true�̂܂܂�
        for (int i = 1; i < m_StateCom.Length; i ++)
        {
            m_StateCom[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cutIn.activeSelf)
            return;

        if (m_frameTime <= 0.0f)
            return;

        if (Hp <= 0)
        {
            if (!m_StateCom[m_nStateNum].AnimEnemy.GetBool("isDeath"))
            {
                if (!IsJust)
                {
                    // ���S�A�j���[�V�����Đ�
                    m_StateCom[m_nStateNum].AnimEnemy.SetBool("isDeath", true);
                    m_StateCom[m_nStateNum].AnimEnemy.Play("Death", 0, 0.55f);
                    //���ݎ����̃~���b�ŃV�[�h�l��������
                    Random.InitState(System.DateTime.Now.Millisecond);
                    int rand = Random.Range(1, 101);
                    if (rand <= DropRate)
                    {
                        // ���S���ɗ��Ƃ��A�C�e���𐶐�
                        Instantiate(DropItemObj, transform.position, Quaternion.identity);
                    }
                }
                else if(IsJust)
                {
                    var seq = DOTween.Sequence();
                    // �v���C���[�̎��q�b�g�X�g�b�v���A���o�Đ���x��������
                    seq.SetDelay(0.12f); // �� �|�C���g�I
                    // �x����҂�����A������ԉ��o���Đ�
                    var backPosition = transform.position + transform.forward.normalized * -2f;
                    seq.Append(transform.DOMove(backPosition, 0.2f).OnStart(() =>
                    {
                        // ���S�A�j���[�V�����Đ�
                        m_StateCom[m_nStateNum].AnimEnemy.speed = 0.7f;
                        m_StateCom[m_nStateNum].AnimEnemy.SetBool("isDeath", true);
                        m_StateCom[m_nStateNum].AnimEnemy.Play("Death", 0, 0.55f);
                    }));
                    if (!IsOnce)
                    {
                        //���ݎ����̃~���b�ŃV�[�h�l��������
                        Random.InitState(System.DateTime.Now.Millisecond);
                        int rand = Random.Range(1, 101);
                        if (rand <= DropRate)
                        {
                            // ���S���ɗ��Ƃ��A�C�e���𐶐�
                            Instantiate(DropItemObj, transform.position, Quaternion.identity);
                        }
                        IsOnce = true;
                        IsJust = false;
                        Debug.Log("�}�K�W�����Ƃ�");
                    }
                }
            }
        }

        // �_���[�W�\�����G���f���Ɣ��̂ŏ����J���������Ɉړ�������
        if (canvas != null)
        {
            Vector3 campos = Camera.main.transform.position;
            Vector3 dis = new Vector3(campos.x - transform.position.x, campos.y - transform.position.y, campos.z - transform.position.z).normalized;

            canvas.transform.position += dis;
            canvas = null;
        }

    }
    
     public void ChangeState(int NewIndex)
    {
        // �ς��Ȃ��Ȃ珈�����Ȃ�
        if (m_nStateNum == NewIndex)
            return;
        m_StateCom[m_nStateNum].enabled = false;
        m_NowState = m_StateCom[NewIndex];
        m_NowState.enabled = true;
        m_nStateNum = NewIndex;
    }

    public void Death()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        Debug.Log("destroy");
        var alfa = mesh.materials[0].GetFloat("_Threshold");
        while (alfa > 0)
        {
            alfa -= 0.005f;
            mesh.materials[0].SetFloat("_Threshold", alfa);
            yield return null;
        }
        FrameManager.Remove(gameObject, "Enemy");
        Destroy(gameObject);
    }

    //public float GetRandMove()
    //{
    //    return RandMove;
    //}

    public GameObject GetPlayer()
    {
        return m_PlayerObj;
    }

    public GameObject GetBullet()
    {
        return BulletObj;
    }

    public float GetBulletInterval()
    {
        return BulletInterval;
    }

    public int GetBulletMaxNum()
    {
        return BulletMaxNum;
    }

    public float GetOmenTime()
    {
        return OmenTime;
    }

    //public float GetStopChaseDis()
    //{
    //    return StopChaseDis;
    //}

    public Vector3 GetLostPos()
    {
        return m_LostPos;
    }

    public float GetStopDis()
    {
        return StopDistance;
    }

    public int GetPower()
    {
        return Power;
    }

    public void Damage(float damage)
    {
        if (Hp <= 0)
            return;

        // �G�t�F�N�g�Đ�
        GetComponent<EffectSample>().PlayEffect("hit 1");

        // �_���[�W�A�j���[�V�����Đ�
        if (m_nStateNum == 2)
            GetComponent<createSen>().EnebleLine();
        m_StateCom[m_nStateNum].AnimEnemy.Play("Damage", 0, 0.24f);

        Hp -= damage;
        if (damage >= 100)
            IsJust = true;

        // �����Ń_���[�W�\���̊֐�
        canvas = Instantiate(Damagecanvas, transform.position, Quaternion.identity);
        canvas.transform.GetComponentInChildren<DamageTexManager>().SetDamageTex(damage);
    }

    public float GetHp()
    {
        return Hp;
    }

    public float GetMaxHp()
    {
        return m_maxHP;
    }
    
    public string GetName()
    {
        return gameObject.transform.parent.name;
    }

    public float GetKickPower()
    {
        return KickPower;
    }

    public void SetLockFlg(bool flg)
    {
        if (IsLockOn == flg)
            return;

        Aimcanvas.SetActive(flg);

        IsLockOn = flg;
    }

    public void StartFrame()
    {
        m_frameTime = 1.0f;
    }

    public void StopFrame()
    {
        m_frameTime = 0.0f;
    }
}
