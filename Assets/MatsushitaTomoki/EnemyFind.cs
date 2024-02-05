using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFind : MonoBehaviour
{
    private string enemyTag = "Enemy"; // Enemy�^�O�̖��O

    void Update()
    {
        FindEnemy();
    }
    public GameObject FindEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        // ���g�̈ʒu
        Vector3 selfPosition = transform.position;

        // �^�O��enemyTag�̑S�ẴQ�[���I�u�W�F�N�g���擾
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            // Enemy�̈ʒu
            Vector3 enemyPosition = enemy.transform.position;

            // ���g��Enemy�̋������v�Z
            float distance = Vector3.Distance(selfPosition, enemyPosition);

            // ���߂�Enemy�����������ꍇ�A�ł��߂�Enemy�Ƃ��čX�V
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        //Debug.Log(closestEnemy);
        return closestEnemy;
    }
    
    // �R�s�y�p
    // GameObject closestEnemy = GetComponent<FindEnemy>().FindEnemy();
}
