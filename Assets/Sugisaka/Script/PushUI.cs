using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushUI : MonoBehaviour
{
    [Header("UI�̐F")]
    [SerializeField] private Color BeforeColor;  // �����O
    [SerializeField] private Color AfterColor;   // ������

    private void Start()
    {
        // ������
        //GetComponent<Image>().color = BeforeColor;
    }

    public void ShotPush()
    {
        // ��������0.1�b�F��ς���
        StartCoroutine(PushCoroutine());
    }

    public void Push()
    {
        // ��������F��ς���
        GetComponent<Image>().color = AfterColor;
    }

    public void Release()
    {
        // ��������F��߂�
        GetComponent<Image>().color = BeforeColor;
    }

    IEnumerator PushCoroutine()
    {
        // ��������F��ς���
        GetComponent<Image>().color = AfterColor;

        // 0.1�b�҂�
        yield return new WaitForSeconds(0.1f);

        // ��������F��߂�
        GetComponent<Image>().color = BeforeColor;
    }

    public void PushTextUI()
    {
        // ��������F��ς���
        GetComponent<Text>().color = AfterColor;
    }

    public void ReleaseTextUI()
    {
        // ��������F��߂�
        GetComponent<Text>().color = BeforeColor;
    }

}
