using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCamera_ : MonoBehaviour
{
    public string targetTag = "Enemy";
    public CinemachineVirtualCamera freeLookCamera;
    string name = "LButton";
    public float max_distance = 50.0f;

    private GameObject targetEnemy;
    private Camera mainCamera;
    private float nearestDistance;
    private GameObject nearestEnemy;
    private GameObject lockOnEnemy;
    private Player playerScript; // プレイヤースクリプトへの参照

    GameObject SoundManager;


    private void Start()
    {
        playerScript = FindObjectOfType<Player>();

        mainCamera = Camera.main;
        nearestEnemy = null;
        targetEnemy = null;
        lockOnEnemy = null;

        SoundManager = GameObject.Find("SoundManager");
    }

    private void Update()
    {
        FindNearestEnemy();
    }

    void FindNearestEnemy()
    {
        nearestEnemy = null;
        nearestDistance = float.MaxValue;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
            {
                Ray ray = new Ray(mainCamera.transform.position, enemy.transform.position - mainCamera.transform.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, max_distance))
                {
                    if (hit.transform.CompareTag(targetTag))
                    {
                        Enemy enemyComponent = hit.transform.GetComponent<Enemy>();

                        if (enemyComponent != null)
                        {
                            float hp = enemyComponent.GetHp();

                            if (hp > 0)
                            {
                                float distance = Vector3.Distance(hit.transform.position, mainCamera.transform.position);

                                if (distance < nearestDistance)
                                {
                                    nearestDistance = distance;
                                    nearestEnemy = hit.transform.gameObject;
                                }
                            }
                        }
                    }
                }
            }
        }
        if ((Input.GetButtonDown(name) || Input.GetMouseButtonDown(2)) && !playerScript.IsJust())
        {
            if (nearestEnemy != null && max_distance > nearestDistance)
            {
                SoundManager.GetComponent<SoundManager>().PlaySE("ロックオン");
                Debug.Log("ロックオン対象: " + nearestEnemy.name + ", Distance: " + nearestDistance);
                targetEnemy = nearestEnemy;
                freeLookCamera.LookAt = nearestEnemy.transform;
                lockOnEnemy = nearestEnemy;
            }
            else
            {
                Debug.Log("ロックオンできる敵が見つかりません。または、遠すぎます");
            }
        }
    }
   
    public bool CanLockOn()
    {
        return max_distance > nearestDistance ? true : false;
    }

    public GameObject GetLockOnEnemy()
    {
        return targetEnemy;
    }
}
