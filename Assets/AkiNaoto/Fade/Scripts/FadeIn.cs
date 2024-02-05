/*
�t�F�[�h�C������ 
�P�D�Q�[���V�[���ɋ�̃Q�[���I�u�W�F�N�g���쐬���A�����ɂ��̃X�N���v�g��ǉ�
�Q�D�q�G�����L�[��FadeCanvas��ǉ����A�C���X�y�N�^�[�r���[�̃}�X�N�e�N�X�`���ɍD���ȃ}�X�N�摜��I��
�R�D��̃Q�[���I�u�W�F�N�g���擾�A�Q�Ƃ���Ȃǂ��Ă��̃X�N���v�g���Ăяo��

 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    [SerializeField] FadeT fade;
    [SerializeField] float FadeTime_Inst;//�t�F�C�h�ɂ����鎞��

    public string NextSceneName;    //���̃V�[����
    public static float FadeTime;   //�t�F�[�h�ɂ����鎞��
    public GameObject Text;

    public static FadeIn instance = null;

    void Awake()
    {
        FadeTime = FadeTime_Inst;
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void NextScene()
    {
        Action act = () => SceneManager.LoadScene(NextSceneName);
        float time = FadeTime;

        //���Ԍo�߂��Ă���ɃV�[���ړ�����
        fade.FadeIn(time, act);
    }

    public void NextScene(string SceneName)
    {

        NextSceneName = SceneName;
        Action act = () => SceneManager.LoadScene(NextSceneName);
        float time = FadeTime;

        //���Ԍo�߂��Ă���ɃV�[���ړ�����
        fade.FadeIn(time, act);
    }

    public void FadeOnly()
    {
        float time = FadeTime;
        Action act = () => Text.GetComponent<Fadetext>().Fadestart();

        //���Ԍo�߂��Ă���ɃV�[���ړ�����
        fade.FadeIn(time, act);
    }
}
