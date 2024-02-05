using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;

    [SerializeField] private GameObject SoundPrefab;

    void Start()
    {
        SoundPrefab = GameObject.Find("SoundManager");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SoundPrefab.GetComponent<SoundManager>().PlaySE("ロール回避1");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SoundPrefab.GetComponent<SoundManager>().PlaySE("ロール回避2");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SoundPrefab.GetComponent<SoundManager>().PlaySE("ステップ回避");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SoundPrefab.GetComponent<SoundManager>().PlaySE("ジャスト回避");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SoundPrefab.GetComponent<SoundManager>().PlaySE("倒れる");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SoundPrefab.GetComponent<SoundManager>().StopSE("走る");
            SoundPrefab.GetComponent<SoundManager>().PlaySE("歩行");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SoundPrefab.GetComponent<SoundManager>().StopSE("歩行");
            SoundPrefab.GetComponent<SoundManager>().PlaySE("走る");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SoundPrefab.GetComponent<SoundManager>().PlaySE("発砲音");
        }
    }
}
