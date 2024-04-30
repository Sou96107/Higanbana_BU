using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FrameManager
{
    private static List<GameObject> gameObjects = new List<GameObject>();
    private static List<string> CSname = new List<string>();

    public static void Add(GameObject gameObject, string csName)
    {
        gameObjects.Add(gameObject);
        CSname.Add(csName);
    }

    public static void Remove(GameObject gameObject, string csName)
    {
        gameObjects.Remove(gameObject);
        CSname.Remove(csName);
    }

    public static void StartFrame()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            if (CSname[i] == "Player")
                gameObjects[i].GetComponent<Player>().StartFrame();
            else if (CSname[i] == "Enemy")
                gameObjects[i].GetComponent<EnemyState>().StartFrame();
            else if (CSname[i] == "EnemyBullet")
                gameObjects[i].GetComponent<EnemyBullet>().StartFrame();
            else if (CSname[i] == "PlayerBullet")
                gameObjects[i].GetComponent<PlayerBullet>().StartFrame();
            else if (CSname[i] == "CutIn")
                gameObjects[i].GetComponent<CutIn>().StartFrame();
            else if (CSname[i] == "EnemyShotManager")
                gameObjects[i].GetComponent<EnemyShotManager>().StartFrame();
        }
    }

    public static void StopFrame()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (CSname[i] == "Player")
                gameObjects[i].GetComponent<Player>().StopFrame();
            else if (CSname[i] == "Enemy")
                gameObjects[i].GetComponent<EnemyState>().StopFrame();
            else if (CSname[i] == "EnemyBullet")
                gameObjects[i].GetComponent<EnemyBullet>().StopFrame();
            else if (CSname[i] == "PlayerBullet")
                gameObjects[i].GetComponent<PlayerBullet>().StopFrame();
            else if (CSname[i] == "CutIn")
                gameObjects[i].GetComponent<CutIn>().StopFrame();
            else if (CSname[i] == "EnemyShotManager")
                gameObjects[i].GetComponent<EnemyShotManager>().StopFrame();
        }
    }
}
