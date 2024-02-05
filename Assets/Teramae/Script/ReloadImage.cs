using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadImage : MonoBehaviour
{
    [SerializeField, Header("��]���x")]
    private float rotSpeed = 150.0f;

    private float curReloadTime;    // ���݂̃����[�h����
    private Vector3 rotVector;      // ��]�̎�
    private float reloadTime;       // �����[�h����
    private bool isOnce;            // ��x���������t���O

    // Start is called before the first frame update
    void Start()
    {
        curReloadTime = 0.0f;
        rotVector = new Vector3(0, 0, -1);
        isOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        curReloadTime += Time.deltaTime; // ���݂̃����[�h���ԍX�V
        transform.Rotate(rotVector * rotSpeed * Time.deltaTime); // �I�u�W�F�N�g����]
        //- ���݂̃����[�h���Ԃ������[�h���Ԃ𒴂�����
        if (curReloadTime > reloadTime)
        {
            isOnce = false; // ��x���������t���O������
            gameObject.SetActive(false); // �Q�[������\��
        }
    }

    public void SetReload(float relTime, bool roll = false)
    {
        if(roll)
        {
            reloadTime = relTime;
            transform.rotation = Quaternion.identity;
        }
        //- ��x���������t���O�������Ȃ�
        else if (!isOnce && !roll)
        {
            isOnce = true; // ��x���������t���O�L����
            gameObject.SetActive(true); // �Q�[�����\��
            curReloadTime = 0.0f; // ���݂̃����[�h���ԏ�����
            reloadTime = relTime; // �����[�h���ԑ��
        }
    }
}
