using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraScript : MonoBehaviour
{
    public string targetTag = "Enemy";
    string name = "LButton"; //�o�^�������O

    [SerializeField] private CinemachineVirtualCameraBase _camera0; // ��I�����̃J����
    [SerializeField] private CinemachineVirtualCameraBase _camera1; // �I�����̃J����

    [SerializeField] private int _unselectedPriority = 0;
    [SerializeField] private int _selectedPriority = 10;

    private bool isCamera1Active = false; // �J����1���A�N�e�B�u���ǂ���

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateCameraPriorities();
    }

    private void Update()
    {
        DetectVisibleEnemies();
    }

    void DetectVisibleEnemies()
    {
        bool isInCamera1Range = false; // �G�l�~�[���J����1�͈͓̔��ɂ��邩�ǂ����������t���O

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            // �G�l�~�[�̈ʒu�����[���h���W����r���[�|�[�g���W�ɕϊ�
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
            {
                isInCamera1Range = true;
                break; // �G�l�~�[��1�ł��͈͓��ɂ���ꍇ�̓��[�v�𔲂���
            }
        }

        if (isCamera1Active)
        {
            if (!isInCamera1Range)
            {
                // �J����1���A�N�e�B�u�ł���A�G�l�~�[���͈͊O�ɂ���ꍇ�A�J����0���A�N�e�B�u�ɐ؂�ւ�
                isCamera1Active = false;
                UpdateCameraPriorities();
            }
        }
        else
        {
            if (isInCamera1Range)
            {
                // �J����0���A�N�e�B�u�ł���A�G�l�~�[���J����1�͈͓̔��ɂ���ꍇ�A�J����1���A�N�e�B�u�ɐ؂�ւ�
                isCamera1Active = true;
                UpdateCameraPriorities();
            }
        }
    }

    private void UpdateCameraPriorities()
    {
        if (isCamera1Active)
        {
            _camera0.Priority = _unselectedPriority;
            _camera1.Priority = _selectedPriority;
        }
        else
        {
            _camera0.Priority = _selectedPriority;
            _camera1.Priority = _unselectedPriority;
        }
    }

    public void CamChange()
    {
        isCamera1Active = !isCamera1Active;
        UpdateCameraPriorities();
    }
}
