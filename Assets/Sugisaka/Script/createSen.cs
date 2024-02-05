using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class createSen : MonoBehaviour
{
    // �e���\�����I�u�W�F�N�g
    [SerializeField] GameObject self;                   // pivot
    [SerializeField] public Transform player;           // �v���C���[

    [Header("�\�����̐F")]
    [SerializeField] Material LineColor;
    [SerializeField] private Color startcolor;
    [SerializeField] private Color endcolor;
    [Range(0, 1)]
    public float colorRange; 
    [Header("���̑���")]
    [SerializeField] Vector2 LineWide = new (0.05f, 0.05f);

    private RaycastHit hit;                             // 
    private float distance = 20f;                       // 
    private Vector3 startpos = new (0, 0, 0);           // 
    private Vector3 targetpos = new (0, 0, 0);          // 
    private Vector3[] positions;                        // 
    LineRenderer lineRenderer;
    

    void Start()
    {
        // ���C�������_���[�̐ݒ�
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //�I�u�W�F�N�g�̍��W���擾
        positions = new Vector3[]
        {
            self.transform.position,
            self.transform.position
        };

        //���̑�����ݒ�
        lineRenderer.startWidth = LineWide.x;
        lineRenderer.endWidth = LineWide.y;

        //�}�e���A���̐ݒ�
        var material = LineColor;
        lineRenderer.material = material;

        // �ŏ��͔�\��
        //lineRenderer.enabled = false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        // �v���C���[�ɑ΂��ă��C���Ƃ΂�
    //        // ���C���΂������̌v�Z�A���K��
    //        Vector3 temp = player.transform.position - self.transform.position;
    //        Vector3 direction = temp.normalized;
    //        // ���C���΂�
    //        Ray ray = new Ray(self.transform.position, direction);
    //        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 0.1f);
    //        // ���C�����������I�u�W�F�N�g�𒲂ׂ�
    //        if (Physics.Raycast(ray.origin, ray.direction * distance, out hit))
    //        {
    //            // ���C�����������ꏊ�ɗ\������\��
    //            // ���ɗ\�������\������Ă���ꍇ�͏ꏊ��ύX����
    //            positions[0] = self.transform.position;
    //            //positions[1] = hit.transform.position;
    //            positions[1] = hit.point;
    //            lineRenderer.SetPositions(positions);
    //        }
    //    }
    //}
    //private void Update()
    //{
    //    LineColor.SetColor("_EmissionColor", Color.Lerp(startcolor, endcolor, colorRange));
    //    lineRenderer.material = LineColor;
    //    positions[0] = self.transform.position;
    //    positions[1] = player.position;
    //    lineRenderer.SetPositions(positions);
    //}

    public void DrawLine()
    {
        // ���C���΂������̌v�Z�A���K��
        Vector3 temp = player.position - self.transform.position;
        Vector3 direction = temp.normalized;

        // ���C���΂�
        Ray ray = new (self.transform.position, direction);
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow, 0.1f);

        // ���C�����������I�u�W�F�N�g�𒲂ׂ�
        if (Physics.Raycast(ray.origin, ray.direction * distance, out hit))
        {
            lineRenderer.enabled = true;
            // ���C�����������ꏊ�ɗ\������\��
            // ���ɗ\�������\������Ă���ꍇ�͏ꏊ��ύX����
            positions[0] = self.transform.position;
            positions[1] = player.position;
            lineRenderer.SetPositions(positions);
        }
    }
    public void DrawLine2()
    {
        Vector3 temp = targetpos - startpos;
        Vector3 direction = temp.normalized;

        // ���C���΂�
        Ray ray = new (self.transform.position, direction);
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow, 0.1f);

        // ���C�����������I�u�W�F�N�g�𒲂ׂ�
        if (Physics.Raycast(ray.origin, ray.direction * distance, out hit))
        {
            positions[0] = self.transform.position;
            positions[1] = hit.point;

            // �e���̕\�����������Ȃ炱����L���ɂ���
            //lineRenderer.SetPositions(positions);
        }
    }


    public void SetPos(Transform Target, Transform startPos)
    {
        player = Target;
        targetpos = player.position;
        startpos = startPos.position;
    }
    public void EnebleLine()
    {
        lineRenderer.enabled = false;
    }
    public void SetLineColor(float time)
    {
        //time *= 
        colorRange = time;
        LineColor.SetColor("_EmissionColor", Color.Lerp(startcolor, endcolor, colorRange));
        lineRenderer.material = LineColor;
    }
}
