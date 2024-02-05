using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Offset : MonoBehaviour
{
    public CharacterController characterController;

    public CinemachineVirtualCamera virtualCamera; // シーン内のCinemachine Virtual Cameraをアサインする

    public Vector3 newOffset; // 新しいFollowOffsetの値
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

        // ジャスト回避演出時にこのメソッドを呼び出すことで、FollowOffsetを変更します
        public void SetFollowOffset()
    {
        //if (virtualCamera != null)
        //{
        //    virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = newOffset;
        //}
    }

    // ジャスト回避演出が終了した際に元のFollowOffsetに戻すメソッド
    public void ResetFollowOffset()
    {
        //if (virtualCamera != null)
        //{
        //    virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = DefaultOffset; // ゼロベクトルに戻す例
        //}
    }
}