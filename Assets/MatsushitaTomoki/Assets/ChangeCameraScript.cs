using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraScript : MonoBehaviour
{
    public string targetTag = "Enemy";
    string name = "LButton"; //登録した名前

    [SerializeField] private CinemachineVirtualCameraBase _camera0; // 非選択時のカメラ
    [SerializeField] private CinemachineVirtualCameraBase _camera1; // 選択時のカメラ

    [SerializeField] private int _unselectedPriority = 0;
    [SerializeField] private int _selectedPriority = 10;

    private bool isCamera1Active = false; // カメラ1がアクティブかどうか

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
        bool isInCamera1Range = false; // エネミーがカメラ1の範囲内にいるかどうかを示すフラグ

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            // エネミーの位置をワールド座標からビューポート座標に変換
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
            {
                isInCamera1Range = true;
                break; // エネミーが1つでも範囲内にいる場合はループを抜ける
            }
        }

        if (isCamera1Active)
        {
            if (!isInCamera1Range)
            {
                // カメラ1がアクティブであり、エネミーが範囲外にいる場合、カメラ0をアクティブに切り替え
                isCamera1Active = false;
                UpdateCameraPriorities();
            }
        }
        else
        {
            if (isInCamera1Range)
            {
                // カメラ0がアクティブであり、エネミーがカメラ1の範囲内にいる場合、カメラ1をアクティブに切り替え
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
