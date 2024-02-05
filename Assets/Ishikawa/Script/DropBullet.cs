using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBullet : DropItem
{
    [SerializeField, Header("’e‚Ì‰ñ•œ”")] int Num;
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
                m_soundmanager.GetComponent<SoundManager>().PlaySE("æ“¾‰¹");
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
