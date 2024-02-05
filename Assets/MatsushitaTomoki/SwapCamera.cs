using UnityEngine;
using Cinemachine;

public class SwapCamera : MonoBehaviour
{
    public CinemachineVirtualCamera freeLookCamera; // CinemachineFreeLook�ւ̎Q��
    public CinemachineFreeLook freeLookCamera2; // CinemachineFreeLook�ւ̎Q��

    private GameObject nearTarget; // ��ԋ߂��G���i�[����ϐ�
    string name = "LButton"; //�o�^�������O

    private void Start()
    {
        // CinemachineFreeLook�ւ̎Q�Ƃ��擾
        if (freeLookCamera == null)
        {
            freeLookCamera = GetComponent<CinemachineVirtualCamera>();
            freeLookCamera2 = GetComponent<CinemachineFreeLook>();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown(name))
        {

            // �t�B�[���h��ɂ���S�G�l�~�[�̏����擾
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
            float minDistance = float.MaxValue;

            // ��ԋ߂��G������
            foreach (GameObject t in targets)
            {
                float distance = Vector3.Distance(transform.position, t.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearTarget = t;
                }
            }

            // ��ԋ߂��G��Look At�^�[�Q�b�g�ɐݒ�
            if (nearTarget != null)
            {
                freeLookCamera.LookAt = nearTarget.transform;
                freeLookCamera2.LookAt = nearTarget.transform;
            }
        }
    }
}
