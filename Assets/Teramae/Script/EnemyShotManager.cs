using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotManager : MonoBehaviour
{
    [SerializeField, Header("ランダム最小値")]
    private float minTime = 0.1f;
    [SerializeField, Header("ランダム最大値")]
    private float maxTime = 2.0f;

    private List<GameObject> enemies = new List<GameObject>();

    enum ManagerState
    {
        Rand,   // ランダム格納
        Count,  // タイムカウント
        Order,  // 指示
    }

    private ManagerState nowState;
    private float orderTime;
    private float currentTime;
    private int enemyNum;
    private float frameTime;

    // Start is called before the first frame update
    void Start()
    {
        nowState = ManagerState.Rand;
        orderTime = 0.0f;
        currentTime = 0.0f;
        enemyNum = 0;
        frameTime = 1.0f;
        FrameManager.Add(gameObject, "EnemyShotManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (frameTime <= 0.0f)
            return;

        if (enemies.Count > 0)
        {
            switch (nowState)
            {
                case ManagerState.Rand:
                    if (enemies.Count == 0)
                    {
                        enemyNum = 0;
                        break;
                    }
                    orderTime = Random.Range(minTime, maxTime);
                    nowState = ManagerState.Count;
                    break;

                case ManagerState.Count:
                    if (enemies.Count == 0)
                    {
                        enemyNum = 0;
                        break;
                    }
                    currentTime += Time.deltaTime;
                    if(currentTime >= orderTime)
                    {
                        nowState = ManagerState.Order;
                        currentTime = 0.0f;
                    }
                    break;

                case ManagerState.Order:
                    if (enemies.Count == 0)
                    {
                        enemyNum = 0;
                        break;
                    }
                    if(enemyNum >= enemies.Count)
                    {
                        enemyNum -= enemyNum - enemies.Count + 1;
                    }
                    if (enemies[enemyNum].transform.GetChild(0).gameObject.GetComponent<EnemyState_Attack>().ChangeState())
                    {
                        nowState = ManagerState.Rand;
                        if (enemyNum < enemies.Count - 1)
                        {
                            enemyNum++;
                        }
                        else
                        {
                            enemyNum = 0;
                        }
                    }
                    break;
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].transform.GetChild(0).gameObject.GetComponent<Enemy>().GetHp() <= 0.0f)
                {
                    Debug.Log("敵死亡によりリスト削除" + enemies[i].name);
                    enemies.Remove(enemies[i]);
                    if (enemyNum > 0)
                        enemyNum--;
                }
            }
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        Debug.Log("攻撃してくる敵加算：" + enemy.name);
    }

    public void SubEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("攻撃してくる敵減算：" + enemies.Count);
    }

    public void StartFrame()
    {
        frameTime = 1.0f;
        Debug.Log("敵銃撃マネージャーフレーム：" + frameTime);
    }

    public void StopFrame()
    {
        frameTime = 0.0f;
        Debug.Log("敵銃撃マネージャーフレーム：" + frameTime);
    }
}
