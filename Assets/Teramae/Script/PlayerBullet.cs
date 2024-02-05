using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField, Header("速度")]
    private float speed = 15.0f;
    [SerializeField, Header("削除時間(秒)")]
    private float destroyCount = 3.0f;

    private Rigidbody rb;
    private Vector3 move;
    private float curDestCount;
    private GameObject player;
    private Vector3 playerPos;
    private GameObject lockonEnemy;
    private bool isJust;

    Vector3 prepos;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        curDestCount = 0.0f;
        prepos = transform.position; //前フレームでの位置
    }

    // Update is called once per frame
    void Update()
    {
        curDestCount += Time.deltaTime;
        if(curDestCount > destroyCount)
        {
            Destroy(gameObject);
        }

        Vector3 pos = transform.position; //今フレームでの位置

        if (!isJust)
        {
            Ray ray = new Ray(prepos, (pos - prepos).normalized); //前フレームの位置から今の位置の向きにRayを飛ばす

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    float distance = Vector3.Distance(playerPos, hit.collider.gameObject.transform.parent.position + (hit.collider.gameObject.transform.position - hit.collider.gameObject.transform.parent.position));
                    float magnitude = (distance / 10.0f);
                    if (magnitude < 1)
                        magnitude += 1.0f;
                    float damage = GameObject.Find("Player").GetComponent<Player>().power - (int)(distance * magnitude);
                    if (damage <= 0)
                        damage = 1;
                    //Debug.Log("距離：" + distance);
                    //Debug.Log("倍率：" + (int)magnitude);
                    hit.collider.gameObject.GetComponent<Enemy>().Damage((int)damage);
                    //Debug.Log("ダメージ量：" + (int)damage);

                    Destroy(gameObject);
                }

                if (hit.collider.CompareTag("Stage") || hit.collider.CompareTag("Obj"))
                {
                    GetComponent<EffectSample>().PlayEffect("Smoke", hit.point);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            lockonEnemy.GetComponent<Enemy>().Damage(GameObject.Find("Player").GetComponent<Player>().power);
            Destroy(gameObject);
        }

        prepos = pos;
    }

    public void SetBullet(float x, float y, float z)
    {
        move.Set(x, y, z);
        rb = GetComponent<Rigidbody>();
        rb.velocity = move * speed;
    }

    public void SetPlayerPos(Vector3 pos)
    {
        playerPos = pos;
    }

    public void SetJustBullet(GameObject enemy)
    {
        lockonEnemy = enemy;
        isJust = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("当たった");

    //    // 当たったのがエネミー
    //    if (collision.transform.tag.Contains("Enemy"))
    //    {
    //        collision.gameObject.GetComponent<Enemy>().Damage(
    //            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().power);
    //    }

    //    Destroy(gameObject);
    //}
}
