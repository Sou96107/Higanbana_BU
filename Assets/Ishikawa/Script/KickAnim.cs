using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAnim : MonoBehaviour
{
    [SerializeField] GameObject KickCollider;
   

    void OnActiveKick()
    {
        KickCollider.SetActive(true);
    }

    void NotActiveKick()
    {
        KickCollider.SetActive(false);
    }
}
