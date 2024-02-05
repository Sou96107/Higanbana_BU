using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnemyDetector : MonoBehaviour
{
    private string targetTag = "Enemy";

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        DetectVisibleEnemies();
    }

    void DetectVisibleEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            // �G�l�~�[�̈ʒu�����[���h���W����r���[�|�[�g���W�ɕϊ�
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
            {
                // �G�l�~�[���J�����̃r���[�|�[�g���ɂ���ꍇ�A���O���o��
            }
        }
    }
}
