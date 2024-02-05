using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadImage : MonoBehaviour
{
    [SerializeField, Header("回転速度")]
    private float rotSpeed = 150.0f;

    private float curReloadTime;    // 現在のリロード時間
    private Vector3 rotVector;      // 回転の軸
    private float reloadTime;       // リロード時間
    private bool isOnce;            // 一度だけ処理フラグ

    // Start is called before the first frame update
    void Start()
    {
        curReloadTime = 0.0f;
        rotVector = new Vector3(0, 0, -1);
        isOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        curReloadTime += Time.deltaTime; // 現在のリロード時間更新
        transform.Rotate(rotVector * rotSpeed * Time.deltaTime); // オブジェクトを回転
        //- 現在のリロード時間がリロード時間を超えたら
        if (curReloadTime > reloadTime)
        {
            isOnce = false; // 一度だけ処理フラグ無効化
            gameObject.SetActive(false); // ゲーム内非表示
        }
    }

    public void SetReload(float relTime, bool roll = false)
    {
        if(roll)
        {
            reloadTime = relTime;
            transform.rotation = Quaternion.identity;
        }
        //- 一度だけ処理フラグが無効なら
        else if (!isOnce && !roll)
        {
            isOnce = true; // 一度だけ処理フラグ有効化
            gameObject.SetActive(true); // ゲーム内表示
            curReloadTime = 0.0f; // 現在のリロード時間初期化
            reloadTime = relTime; // リロード時間代入
        }
    }
}
