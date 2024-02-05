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

        // iƒ|ƒCƒ“ƒgj
        // ‚U‚OƒtƒŒ[ƒ€‚²‚Æ‚É–C’e‚ğ”­Ë‚·‚é
        if (count % frame == 0)
        {
            GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();

            // ’e‘¬‚Í©—R‚Éİ’è
            shellRb.AddForce(transform.forward * 500);

            // ‚T•bŒã‚É–C’e‚ğ”j‰ó‚·‚é
            Destroy(shell, 5.0f);
        }
    }
}
