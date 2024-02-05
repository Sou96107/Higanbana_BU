using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;


public class DefaltTargetCamera : MonoBehaviour
{
    public string targetTag = "Enemy";
    public CinemachineVirtualCamera freeLookCamera; // CinemachineFreeLook�ւ̎Q��
    string name = "LButton"; // �o�^�������O

    private GameObject targetEnemy;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        // CinemachineFreeLook�ւ̎Q�Ƃ��擾
        if (freeLookCamera == null)
        {
            freeLookCamera = GetComponent<CinemachineVirtualCamera>();
        }
        GameObject nearestEnemy = null;
        targetEnemy = null;
    }

    private void Update()
    {
        FindNearestEnemy();
    }

    void FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            // �G�l�~�[�̈ʒu�����[���h���W����r���[�|�[�g���W�ɕϊ�
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
            {
                // �G�l�~�[���J�����̃r���[�|�[�g���ɂ���ꍇ�A�������v�Z
                float distance = Vector3.Distance(enemy.transform.position, mainCamera.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        if (Input.GetButtonDown(name))
        {
            Debug.Log("���b�N�I���Ώ�: " + nearestEnemy.name);
            targetEnemy = nearestEnemy;
            freeLookCamera.LookAt = nearestEnemy.transform;
        }

    }

    public GameObject GetLockOnEnemy()
    {
        return targetEnemy;
    }
}