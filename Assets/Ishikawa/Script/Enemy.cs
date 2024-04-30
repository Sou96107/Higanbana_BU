using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("体力")]  float Hp = 3.0f;
    [SerializeField, Header("歩行スピード")] float Speed = 5.0f;
    [SerializeField, Header("予測線を出す秒数")]  float OmenTime = 1.0f;
    //[SerializeField, Header("徘徊時の移動量")]  float RandMove = 5.0f;
    [SerializeField, Header("プレイヤーに近づくのをやめる距離")] float StopDistance = 1.0f;
    //[SerializeField, Header("追うのをやめる距離")]  float StopChaseDis = 20.0f;
    [SerializeField, Header("攻撃力")] int Power = 1;
    [SerializeField, Header("蹴りの威力")] float KickPower = 5.0f;
    [SerializeField, Header("弾丸オブジェクト")] public GameObject BulletObj;
    [SerializeField, Header("弾発射インターバル(秒数)")] float BulletInterval = 0.5f;
    [SerializeField, Header("弾丸数")] int BulletMaxNum = 1;
    [SerializeField, Header("落とすアイテム")] GameObject DropItemObj;
    [SerializeField, Range(0, 100), Header("落とす確率(％)")] int DropRate = 25;
    [SerializeField, Header("ノズルフラッシュオブジェ")] public GameObject MuzzleFlashObj;
    [SerializeField,Header("ダメージ表示キャンバス")] GameObject Damagecanvas;
    [SerializeField, Header("照準表示キャンバス")] GameObject Aimcanvas;
    [SerializeField, Header("モデルオブジェクト")] public GameObject enemyModel;
    [SerializeField, Header("ノズルオブジェクト")] public Transform MuzzleTrs;
    [SerializeField, Header("カットインオブジェクト")] public GameObject cutIn;
    [SerializeField, Header("メッシュ")] SkinnedMeshRenderer mesh;
    EnemyState[] m_StateCom;  // StateComponentの配列
    [SerializeField] EnemyState m_NowState;  // 現在のState

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
        // 変数に値代入
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

        //m_NowState = gameObject.AddComponent<EnemyState_None>(); // 最初はNoneState
        // ここで使うStateコンポーネント追加
        m_StateCom[0] = gameObject.AddComponent<EnemyState_None>(); // 何もしない
        m_StateCom[1] = gameObject.AddComponent<EnemyState_Chase>(); // 追跡
        m_StateCom[2] = gameObject.AddComponent<EnemyState_Attack>(); // 攻撃 
        m_NowState = m_StateCom[0];
        Debug.Log(m_NowState);
        
        // 最初はNoneStateだけenableをtrueのままに
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
                    // 死亡アニメーション再生
                    m_StateCom[m_nStateNum].AnimEnemy.SetBool("isDeath", true);
                    m_StateCom[m_nStateNum].AnimEnemy.Play("Death", 0, 0.55f);
                    //現在時刻のミリ秒でシード値を初期化
                    Random.InitState(System.DateTime.Now.Millisecond);
                    int rand = Random.Range(1, 101);
                    if (rand <= DropRate)
                    {
                        // 死亡時に落とすアイテムを生成
                        Instantiate(DropItemObj, transform.position, Quaternion.identity);
                    }
                }
                else if(IsJust)
                {
                    var seq = DOTween.Sequence();
                    // プレイヤーの持つヒットストップ分、演出再生を遅延させる
                    seq.SetDelay(0.12f); // ← ポイント！
                    // 遅延を待った後、吹っ飛ぶ演出を再生
                    var backPosition = transform.position + transform.forward.normalized * -2f;
                    seq.Append(transform.DOMove(backPosition, 0.2f).OnStart(() =>
                    {
                        // 死亡アニメーション再生
                        m_StateCom[m_nStateNum].AnimEnemy.speed = 0.7f;
                        m_StateCom[m_nStateNum].AnimEnemy.SetBool("isDeath", true);
                        m_StateCom[m_nStateNum].AnimEnemy.Play("Death", 0, 0.55f);
                    }));
                    if (!IsOnce)
                    {
                        //現在時刻のミリ秒でシード値を初期化
                        Random.InitState(System.DateTime.Now.Millisecond);
                        int rand = Random.Range(1, 101);
                        if (rand <= DropRate)
                        {
                            // 死亡時に落とすアイテムを生成
                            Instantiate(DropItemObj, transform.position, Quaternion.identity);
                        }
                        IsOnce = true;
                        IsJust = false;
                        Debug.Log("マガジン落とし");
                    }
                }
            }
        }

        // ダメージ表示が敵モデルと被るので少しカメラ方向に移動させる
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
        // 変わらないなら処理しない
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

        // エフェクト再生
        GetComponent<EffectSample>().PlayEffect("hit 1");

        // ダメージアニメーション再生
        if (m_nStateNum == 2)
            GetComponent<createSen>().EnebleLine();
        m_StateCom[m_nStateNum].AnimEnemy.Play("Damage", 0, 0.24f);

        Hp -= damage;
        if (damage >= 100)
            IsJust = true;

        // ここでダメージ表示の関数
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
