using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoControll : MonoBehaviour
{
    public GameObject objectToExclude; // �������~�߂Ȃ��I�u�W�F�N�g
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != objectToExclude)
        {
            // ����̃I�u�W�F�N�g�ȊO�̏ꍇ�A�������~����
            return;
        }
    }
}
