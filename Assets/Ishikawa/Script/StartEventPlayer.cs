using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEventPlayer : MonoBehaviour
{
    [SerializeField, Header("何秒動くか")] float MoveTime = 1.0f;
    [SerializeField, Header("モデル")] GameObject model;
    [SerializeField, Header("サウンドマネージャー")] GameObject SoundManager;
    float m_CountTime;
    bool m_Finish;
    Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        m_CountTime = 0.0f;
        m_Finish = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 本来はここでリロードモーションを一秒間ほどかけて行う
        // リロードアニメーション
        


        if (m_CountTime >= MoveTime)
        {
            m_Finish = true;
        }

        m_CountTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        playerAnim = model.GetComponent<Animator>();
        if (!playerAnim.GetBool("isReroad"))
        {
            playerAnim.SetBool("isReload", true);
            playerAnim.Play("Reload");


            if (SoundManager == null)
                SoundManager = GameObject.Find("SoundManager");
            // SE再生
            SoundManager.GetComponent<SoundManager>().PlaySE("リロード");
        }
    }

    public bool GetFinish()
    {
        return m_Finish;
    }
}
