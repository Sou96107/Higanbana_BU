using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeMesh : MonoBehaviour
{
    [SerializeField]
    GameObject BaseMeshObj;

    [SerializeField]
    GameObject BakeClone;

    private List<GameObject> BakeList = new List<GameObject>();

    public void AfterImagef(Transform tr)
    {
        // ���f���A���W�擾�A�ݒ�
        BakeClone = Instantiate(BaseMeshObj);
        BakeClone.transform.position = tr.position;
        BakeClone.transform.rotation = tr.rotation;
        BakeClone.GetComponent<CapsuleCollider>().enabled = false;
        // �A�j���[�V�����̐ݒ�
        BakeClone.GetComponent<Animator>().SetBool("isMove", true);
        // �A�j���[�V�����̒�~
        BakeClone.GetComponent<Animator>().speed = 0.0f;
    }

    public void AfterImages(Transform tr, float time)
    {
        StartCoroutine(AfterImageCoroutine(tr, time));
    }

    IEnumerator AfterImageCoroutine(Transform tr, float time)
    {
        yield return new WaitForSeconds(0.15f);

        // ���f���A���W�擾�A�ݒ�
        BakeList.Add(Instantiate(BaseMeshObj));
        BakeList[BakeList.Count - 1].transform.position = tr.position;
        BakeList[BakeList.Count - 1].transform.rotation = tr.rotation;
        BakeList[BakeList.Count - 1].GetComponent<CapsuleCollider>().enabled = false;
        // �A�j���[�V�����̐ݒ�
        BakeList[BakeList.Count - 1].GetComponent<Animator>().Play("JustStep", 0, time);
        // �A�j���[�V�����̒�~
        BakeList[BakeList.Count - 1].GetComponent<Animator>().speed = 0.0f;

        //BakeClone = Instantiate(BaseMeshObj);
        //BakeClone.transform.position = tr.position;
        //BakeClone.transform.rotation = tr.rotation;
        //BakeClone.GetComponent<CapsuleCollider>().enabled = false;
        //// �A�j���[�V�����̐ݒ�
        //BakeClone.GetComponent<Animator>().Play("JustStep", 0, time);
        //// �A�j���[�V�����̒�~
        //BakeClone.GetComponent<Animator>().speed = 0.0f;
    }

    public void DestroyBakeMesh()
    {
        Destroy(BakeClone);
        for(int i = 0; i < BakeList.Count; i++)
        {
            Destroy(BakeList[i]);
        }
    }
}
