using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RecoilTarget : MonoBehaviour
{
    public CinemachineVirtualCamera freeLookCamera;
    public GameObject DafaultObject;
    public GameObject recoil;

    private Player playerScript; // �v���C���[�X�N���v�g�ւ̎Q��

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�X�N���v�g���������ĎQ�Ƃ��擾
        playerScript = FindObjectOfType<Player>();

        if (playerScript == null)
        {
            Debug.Log("Player script not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript != null && playerScript.IsShot())
        {
            freeLookCamera.LookAt = recoil.transform;
        }
        else
        {
            freeLookCamera.LookAt = DafaultObject.transform;
        }
    }
}
