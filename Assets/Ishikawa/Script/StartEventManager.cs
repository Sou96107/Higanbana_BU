using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEventManager : MonoBehaviour
{
    [SerializeField, Header("MissionStart�e�L�X�g�W")] GameObject[] texs;
    [SerializeField, Header("������Active�ɂ���Ԋu")] float interval = 0.1f;
    [SerializeField, Header("Player")] GameObject PlayerObj;
    [SerializeField, Header("�J����")] GameObject cameraObj;
    [SerializeField, Header("�}�e���A��(�瑩)")]
    private Material chisatoMat;
    enum State
    {
        TexMove,
        PlayerMove,
        SceneChange
    }
    State m_NowState;
    float m_CountTime;
    int m_Num;
    MissionMove m_MoveCom;
    StartEventPlayer m_PlayerCom;
    bool IsStart;
    Color color;
    // Start is called before the first frame update

    private void Awake()
    {
        color = chisatoMat.color;
        color.a = 1.0f;
        chisatoMat.color = color;
    }

    void Start()
    {
        m_NowState = State.TexMove;
        m_CountTime = 0.0f;
        m_Num = 0;
        m_MoveCom = texs[texs.Length - 1].GetComponent<MissionMove>();
        m_PlayerCom = PlayerObj.GetComponent<StartEventPlayer>();
        IsStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStart)
        {
            m_CountTime += Time.deltaTime;
            if(m_CountTime >= 1.5f)
            {
                cameraObj.GetComponent<StartEventCamera>().enabled = true;
                m_CountTime = 0.0f;
                IsStart = true;
            }
            return;
        }

        switch (m_NowState)
        {
            case State.TexMove:
                if (m_Num < texs.Length)
                {
                    if (m_CountTime >= interval)
                    {
                        texs[m_Num].SetActive(true);
                        m_CountTime = 0.0f;
                        m_Num++;
                    }
                }
                else // �Ō�܂ŕ\��������
                {
                    if (m_MoveCom.GetFinish()) // �Ō�̕������ړ��I��������
                    {

                        m_PlayerCom.enabled = true;
                        m_NowState = State.PlayerMove;
                    }
                }
                m_CountTime += Time.deltaTime;
                break;
            case State.PlayerMove:
                if (m_PlayerCom.GetFinish())
                    m_NowState = State.SceneChange;
                break;
            case State.SceneChange:
                FadeIn.instance.NextScene("Game");
                Destroy(gameObject);
                break;
        }
        
        
    }
}
