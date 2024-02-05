using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMove : MonoBehaviour
{
    [SerializeField, Header("��]�̒��S�_")]
    private GameObject centerObj;
    [SerializeField, Header("�t�F�[�h�C���[�W")]
    private CanvasGroup fade;
    [SerializeField, Header("�t�F�[�h�J�n����")]
    private float fadeStartTime = 1.0f;
    [SerializeField, Header("�t�F�[�h���x")]
    private float fadeTime = 1.0f;
    [SerializeField, Header("�N���AUI�C���[�W�@")]
    private RectTransform clearUI_RT;
    [SerializeField, Header("�N���AUI�C���[�W�A")]
    private CanvasGroup clearUI_CG;
    [SerializeField, Header("�N���AUI�T�C�Y(�ŏ�)")]
    private float clearUI_minSize;
    [SerializeField, Header("�N���AUI�T�C�Y(����)")]
    private float clearUI_normalSize;

    private float curFadeTime; // �t�F�[�h�܂ł̌��ݎ���
    private Vector3 currentSize; // 
    private float addSize;
    private bool isAdd;
    private bool isCalc;
    private bool isOnce;

    // Start is called before the first frame update
    void Start()
    {
        curFadeTime = 0.0f;
        currentSize = clearUI_RT.localScale;
        addSize = clearUI_RT.localScale.x;
        isAdd = false;
        isCalc = true;
        isOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���f���̈ړ�
        transform.RotateAround(centerObj.transform.position, Vector3.up, -360 / 45 * Time.deltaTime);
        // �o�ߎ��Ԃ̍X�V
        curFadeTime += Time.deltaTime;

        if(curFadeTime > fadeStartTime)
        {
            // �A���t�@�l�̍X�V
            fade.alpha += fadeTime * Time.deltaTime;
            clearUI_CG.alpha += fadeTime * Time.deltaTime;

            if (isCalc)
            {
                if (!isAdd)
                {
                    if (addSize > clearUI_minSize)
                        addSize += (clearUI_minSize - currentSize.x) * 4 * Time.deltaTime;
                    if (addSize <= clearUI_minSize)
                    {
                        currentSize = clearUI_RT.localScale;
                        isAdd = true;
                    }
                }
                else
                {
                    if (addSize < clearUI_normalSize)
                        addSize += (clearUI_normalSize - currentSize.x) * 4 * Time.deltaTime;
                    if (addSize >= clearUI_normalSize)
                        isCalc = false;
                }
                clearUI_RT.localScale = new Vector3(addSize, addSize, addSize);
            }
        }
        if (curFadeTime > 5.5f)
        {
            if (!isOnce)
            {
                isOnce = true;
                FadeIn.instance.NextScene("Title");
            }
        }
    }
}
