using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPos : MonoBehaviour
{
    public Transform followTarget;
    public float RecoilPos = 0.2f;
    private Player playerScript; // プレイヤースクリプトへの参照


    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        // Transform.positionは読み取り専用なので、新しい位置を作成して代入します
        Vector3 newPosition = followTarget.position;
        if (playerScript != null && playerScript.IsShot())
        {
            newPosition.y += RecoilPos;
        }
        transform.position = newPosition;
    }
}
