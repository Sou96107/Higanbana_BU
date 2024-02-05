using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField, Header("������UI")]
    private RectTransform canvasRectTfm;
    [SerializeField, Header("�v���C���[")]
    private Transform targetTfm;
    [SerializeField, Header("�I�t�Z�b�g")]
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    private RectTransform myRectTfm;    // �X�^�~�i�o�[�̏��i�[

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 pos;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTfm.position + offset);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTfm, screenPos, Camera.main, out pos);

        myRectTfm.position = pos;
    }

}
