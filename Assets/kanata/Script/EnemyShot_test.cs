using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot_test : MonoBehaviour
{
    public GameObject shellPrefab;
    private int count;
    public int frame = 60;

    void Update()
    {
        count += 1;

        // �i�|�C���g�j
        // �U�O�t���[�����ƂɖC�e�𔭎˂���
        if (count % frame == 0)
        {
            GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            // �e���͎��R�ɐݒ�
            shellRb.AddForce(transform.forward * 500);

            // �T�b��ɖC�e��j�󂷂�
            Destroy(shell, 5.0f);
        }
    }
}
