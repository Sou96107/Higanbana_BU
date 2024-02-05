using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ChangeCamera : MonoBehaviour
{
    public string targetTag = "Enemy";
    string name = "LButton"; //登録した名前

    [SerializeField] private CinemachineVirtualCameraBase _camera0; // 非選択時のカメラ
    [SerializeField] private CinemachineVirtualCameraBase _camera1; // 選択時のカメラ

    [SerializeField] private int _unselectedPriority = 0;
    [SerializeField] private int _selectedPriority = 10;

    private bool isCamera1Active = false; // カメラ1がアクティブかどうか

    private Camera mainCamera;
    private bool CanChange;
    private GameObject lockonEnemy;
    private Player player_CS;

    private void Start()
    {
        CanChange = false;
        mainCamera = Camera.main;
        UpdateCameraPriorities();
        player_CS = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        DetectVisibleEnemies();

        if (isCamera1Active)
        {
            GameObject lookAtObject = _camera1.LookAt.gameObject;
            Enemy enemyComponent = lookAtObject.GetComponent<Enemy>();

            if (enemyComponent != null)
            {
                float hp = enemyComponent.GetHp();
                if (hp <= 0)
                {
                    CamChange();
                }
            }
        }
    }

    void DetectVisibleEnemies()
    {
        TargetCamera_ targetCameraComponent = GetComponent<TargetCamera_>();
        if (targetCameraComponent != null && (Input.GetButtonDown(name) || Input.GetMouseButtonDown(2)) && targetCameraComponent.CanLockOn() && !player_CS.IsJust())
        {

            bool once = false;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
            {
                // エネミーの位置をワールド座標からビューポート座標に変換
                Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);

                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    once = true;
                }
            }
            if(once)
            {
                // エネミーがカメラのビューポート内にある場合、ログを出力
                isCamera1Active = !isCamera1Active;
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
            //- プレイヤーのロックオン対象にターゲットカメラの敵情報格納
            GameObject.Find("Player").GetComponent<Player>().SetLockonEnemy(GetComponent<TargetCamera_>().GetLockOnEnemy(), true);
            //- ロックオン対象にレティクル表示
            lockonEnemy = GetComponent<TargetCamera_>().GetLockOnEnemy();
            lockonEnemy.GetComponent<Enemy>().SetLockFlg(true);
        }
        else
        {
            _camera0.Priority = _selectedPriority;
            _camera1.Priority = _unselectedPriority;
            //- プレイヤーのロックオン対象を初期化
            GameObject.Find("Player").GetComponent<Player>().SetLockonEnemy(null, false);
            //- ロックオン対象にレティクル非表示
            if (lockonEnemy)
                lockonEnemy.GetComponent<Enemy>().SetLockFlg(false);
        }
    }

    public void CamChange()
    {
        isCamera1Active = !isCamera1Active;
        UpdateCameraPriorities();
    }
}
