using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFind : MonoBehaviour
{
    private string enemyTag = "Enemy"; // Enemyタグの名前

    void Update()
    {
        FindEnemy();
    }
    public GameObject FindEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        // 自身の位置
        Vector3 selfPosition = transform.position;

        // タグがenemyTagの全てのゲームオブジェクトを取得
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            // Enemyの位置
            Vector3 enemyPosition = enemy.transform.position;

            // 自身とEnemyの距離を計算
            float distance = Vector3.Distance(selfPosition, enemyPosition);

            // より近いEnemyが見つかった場合、最も近いEnemyとして更新
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        //Debug.Log(closestEnemy);
        return closestEnemy;
    }
    
    // コピペ用
    // GameObject closestEnemy = GetComponent<FindEnemy>().FindEnemy();
}
