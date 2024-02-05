using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPos : MonoBehaviour
{
    public Transform followTarget;
    public float RecoilPos = 0.2f;
    private Player playerScript; // �v���C���[�X�N���v�g�ւ̎Q��


    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        // Transform.position�͓ǂݎ���p�Ȃ̂ŁA�V�����ʒu���쐬���đ�����܂�
        Vector3 newPosition = followTarget.position;
        if (playerScript != null && playerScript.IsShot())
        {
            newPosition.y += RecoilPos;
        }
        transform.position = newPosition;
    }
}
