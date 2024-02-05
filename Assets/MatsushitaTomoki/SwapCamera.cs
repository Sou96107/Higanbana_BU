using UnityEngine;
using Cinemachine;

public class SwapCamera : MonoBehaviour
{
    public CinemachineVirtualCamera freeLookCamera; // CinemachineFreeLookへの参照
    public CinemachineFreeLook freeLookCamera2; // CinemachineFreeLookへの参照

    private GameObject nearTarget; // 一番近い敵を格納する変数
    string name = "LButton"; //登録した名前

    private void Start()
    {
        // CinemachineFreeLookへの参照を取得
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

            // フィールド上にいる全エネミーの情報を取得
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
            float minDistance = float.MaxValue;

            // 一番近い敵を検索
            foreach (GameObject t in targets)
            {
                float distance = Vector3.Distance(transform.position, t.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearTarget = t;
                }
            }

            // 一番近い敵をLook Atターゲットに設定
            if (nearTarget != null)
            {
                freeLookCamera.LookAt = nearTarget.transform;
                freeLookCamera2.LookAt = nearTarget.transform;
            }
        }
    }
}
