using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Offset : MonoBehaviour
{
    public CharacterController characterController;

    public CinemachineVirtualCamera virtualCamera; // �V�[������Cinemachine Virtual Camera���A�T�C������

    public Vector3 newOffset; // �V����FollowOffset�̒l
    public Vector3 DefaultOffset;

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            SetFollowOffset();
        }
        if (Input.GetKey(KeyCode.Y))
        {
            ResetFollowOffset();
        }
    }

        // �W���X�g������o���ɂ��̃��\�b�h���Ăяo�����ƂŁAFollowOffset��ύX���܂�
        public void SetFollowOffset()
    {
        //if (virtualCamera != null)
        //{
        //    virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = newOffset;
        //}
    }

    // �W���X�g������o���I�������ۂɌ���FollowOffset�ɖ߂����\�b�h
    public void ResetFollowOffset()
    {
        //if (virtualCamera != null)
        //{
        //    virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = DefaultOffset; // �[���x�N�g���ɖ߂���
        //}
    }
}