using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Recoil : MonoBehaviour
{
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript != null && playerScript.IsShot())
        {
            var impulseSource = GetComponent<CinemachineImpulseSource>();
            impulseSource.GenerateImpulse();
        }
    }
}
