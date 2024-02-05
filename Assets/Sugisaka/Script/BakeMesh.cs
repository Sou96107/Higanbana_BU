using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeMesh : MonoBehaviour
{
    [SerializeField]
    GameObject BaseMeshObj;

    [SerializeField]
    GameObject BakeClone;

    public void AfterImagef(Transform tr)
    {
        // モデル、座標取得、設定
        BakeClone = Instantiate(BaseMeshObj);
        BakeClone.transform.position = tr.position;
        BakeClone.transform.rotation = tr.rotation;
        BakeClone.GetComponent<CapsuleCollider>().enabled = false;
        // アニメーションの設定
        BakeClone.GetComponent<Animator>().SetBool("isMove", true);
        // アニメーションの停止
        BakeClone.GetComponent<Animator>().speed = 0.0f;
    }

    public void AfterImages(Transform tr, float time)
    {
        StartCoroutine(AfterImageCoroutine(tr, time));
    }

    IEnumerator AfterImageCoroutine(Transform tr, float time)
    {
        yield return new WaitForSeconds(0.15f);

        // 前の残像を消す
        Destroy(BakeClone);
        // モデル、座標取得、設定
        BakeClone = Instantiate(BaseMeshObj);
        BakeClone.transform.position = tr.position;
        BakeClone.transform.rotation = tr.rotation;
        BakeClone.GetComponent<CapsuleCollider>().enabled = false;
        // アニメーションの設定
        BakeClone.GetComponent<Animator>().Play("JustStep", 0, time);
        // アニメーションの停止
        BakeClone.GetComponent<Animator>().speed = 0.0f;
    }

    public void DestroyBakeMesh()
    {
        Destroy(BakeClone);
    }
}
