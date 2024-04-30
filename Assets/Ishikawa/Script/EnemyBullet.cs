using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField, Header("スピード")]  float m_Speed = 100.0f;
    [SerializeField, Header("削除時間(秒)")]
    private float destroyCount = 3.0f;
    Vector3 origine;
    [System.NonSerialized] public int m_Power = 0;
    private Vector3 move;
    private float curDestCount;
    private string enemyName;
    private float frameTime;

    Vector3 prepos;
    RaycastHit hit;
    //bool IsHit;
    // Start is called before the first frame update
    void Start()
    {
        origine = transform.position;
        curDestCount = 0.0f;
        frameTime = 1.0f;
        FrameManager.Add(gameObject, "EnemyBullet");

        prepos = transform.position; //前フレームでの位置
        //IsHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (frameTime <= 0.0f)
            return;

        Vector3 pos = transform.position; //今フレームでの位置

        Ray ray = new Ray(prepos, (pos - prepos).normalized); //前フレームの位置から今の位置の向きにRayを飛ばす
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.transform.tag.Contains("Player"))
            {
                hit.collider.gameObject.transform.parent.GetComponent<Player>().Damage(m_Power);

                // hitの場所にヒットエフェクト

                Destroy(gameObject);
            }

            if (hit.collider.CompareTag("Stage") || hit.collider.CompareTag("Obj"))
            {
                GetComponent<EffectSample>().PlayEffect("Smoke", hit.point);
                Destroy(gameObject);
            }
        }



        prepos = pos;

        // 予測線の表示
        //GetComponent<createSen>().DrawLine2();

        //transform.position += transform.forward * m_Speed * Time.deltaTime;

        //float Distance = Vector3.Distance(transform.position, origine);
        //if (Distance >= m_FlyingDis)
        //    Destroy(gameObject);

        curDestCount += Time.deltaTime;
        if (curDestCount > destroyCount)
        {
            Destroy(gameObject);
        }
    }


    public void SetBullet(float x, float y, float z)
    {
        move.Set(x, y, z);
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        rb.velocity = move * m_Speed;
    }
    public void SetPower(int power)
    {
        m_Power = power;
    }
    public void SetEnemyName(string name)
    {
        enemyName = name;
    }
    public string GetEnemyName()
    {
        return enemyName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Contains("Enemy"))
            return;


        if (other.transform.parent != null && other.transform.parent.tag.Contains("Enemy"))
            return;

        // 親がプレイヤー
        if (other.transform.parent != null && other.transform.parent.tag.Contains("Player"))
        {
            if (!other.transform.tag.Contains("Player"))
                return;
            else
            {
                other.transform.parent.GetComponent<Player>().Damage(m_Power);
                other.transform.parent.GetComponent<Player>().SetEnemyBullet(null);
            }

        }

        // ヒットエフェクト

        // 消す
        Destroy(gameObject);
    }

    public void StartFrame()
    {
        frameTime = 1.0f;
        Debug.Log("敵銃弾フレーム：" + frameTime);
    }

    public void StopFrame()
    {
        frameTime = 0.0f;
        Debug.Log("敵銃弾フレーム：" + frameTime);
    }
}
