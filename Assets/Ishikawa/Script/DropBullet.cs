using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBullet : DropItem
{
    [SerializeField, Header("弾の回復数")] int Num;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_IsContact)
        {
            if(m_Player.GetComponent<Player>().SetMagazine())
            {
                m_soundmanager.GetComponent<SoundManager>().PlaySE("取得音");
                Destroy(gameObject);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
