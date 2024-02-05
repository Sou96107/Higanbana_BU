using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;


public class DefaltTargetCamera : MonoBehaviour
{
    public string targetTag = "Enemy";
    public CinemachineVirtualCamera freeLookCamera; // CinemachineFreeLookへの参照
    string name = "LButton"; // 登録した名前

    private GameObject targetEnemy;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        // CinemachineFreeLookへの参照を取得
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
            // エネミーの位置をワールド座標からビューポート座標に変換
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
            {
                // エネミーがカメラのビューポート内にある場合、距離を計算
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
            Debug.Log("ロックオン対象: " + nearestEnemy.name);
            targetEnemy = nearestEnemy;
            freeLookCamera.LookAt = nearestEnemy.transform;
        }

    }

    public GameObject GetLockOnEnemy()
    {
        return targetEnemy;
    }
}