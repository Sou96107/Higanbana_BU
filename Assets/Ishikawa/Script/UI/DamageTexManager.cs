using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTexManager : MonoBehaviour
{
    [SerializeField, Header("表示するTex")] GameObject tex;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamageTex(float damage)
    {
        GameObject obj;
        float posX = transform.position.x;
        float posY = transform.position.y;
        float x = Random.Range(posX - 1.0f, posX + 1.0f);
        float y = Random.Range(posY - 0.3f, posY + 0.3f);
        obj = Instantiate(tex, new Vector3(x,y,transform.position.z), Quaternion.identity);
        obj.transform.SetParent(transform);
        // 数値セット
        obj.GetComponent<DamageTex2>().SetDamage(damage);
    }
}
