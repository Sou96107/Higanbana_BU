using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clear : MonoBehaviour
{
    [Header("��]�̒��S�_")]
    [SerializeField] GameObject centerObj; 
    [Header("�A�j���[�^�[")]
    [SerializeField] Animator anim;
    [Header("�G�t�F�N�g�J�n����")]
    [SerializeField] float EffectStartTime = 3.0f;
    [SerializeField] SoundManager SoundMan;


    [SerializeField] private float curTime;
    private bool isBack;
    private bool isOnce;

    private void Start()
    {
        curTime = 0.0f;
        isBack = false;
        isOnce = false;
        SoundMan = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        // ���f���̈ړ�
        transform.RotateAround(centerObj.transform.position, Vector3.up, -360 / 45 * Time.deltaTime);
        // �o�ߎ��Ԃ̍X�V
        curTime += Time.deltaTime;
        
        if (!isBack && curTime > EffectStartTime)
        {
            SoundMan.PlaySE("�N���A");
            anim.SetTrigger("StartUI");
            isBack = true;
        }

        if (curTime > 5.5f)
        {
            if (!isOnce)
            {
                isOnce = true;
                FadeIn.instance.NextScene("Title");
            }
        }
    }
}
