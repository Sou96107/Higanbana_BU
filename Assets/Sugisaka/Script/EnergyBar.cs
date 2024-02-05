using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField, Header("�G�l���M�[�񕜎���")]
    float EnergyHealTime = 5.0f;
    [Header("�G�l���M�[�����")]
    [SerializeField] float Step = 0.1f;        // �X�e�b�v���̏����
    [SerializeField] float Roll = 0.1f;        // ���[�����̏����

    GameObject player;    // �v���C���[���i�[
    [Range(0, 100)] public float energy = 100;      // ���݂̃G�l���M�[�i0����100�͈̔͂Őݒ�j
    private float energy_ratio;                     // ���݂̃G�l���M�[���ő�G�l���M�[�Ŋ������䗦
    private Image image_Energygauge;                // �G�l���M�[�Q�[�W��UI�C���[�W
    private float maxEnergy = 100;                  // �v���C���[�̍ő�G�l���M�[
    [HideInInspector] public float SubDash = 1.0f;                   // �_�b�V�����̃G�l���M�[�����
    [HideInInspector] public float SubStep = 0.1f;                   // �X�e�b�v���̃G�l���M�[�����
    [HideInInspector] public float SubRoll = 0.1f;                   // ���[�����̃G�l���M�[�����
    [HideInInspector] public bool isStep = false;                    // �X�e�b�v�����ǂ���
    [HideInInspector] public bool isRoll = false;                    // ���[�������ǂ���

    void Start()
    {
        // �v���C���[�����擾
        //player = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.Find("Player");

        // �G�l���M�[�Q�[�W��UI�C���[�W���擾
        image_Energygauge = gameObject.GetComponent<Image>();

        // �G�l���M�[�ő�l�̐ݒ�
        maxEnergy = EnergyHealTime * 60;

        // ���[�V�������̃G�l���M�[����ʂ��v�Z
        SubStep = maxEnergy * Step;
        SubRoll = maxEnergy * Roll;

        energy = maxEnergy;
    }

    void FixedUpdate()
    {
        //-�G�l���M�[�l�X�V
        //if (player.GetComponent<Player>().isDash) 
        //{// �_�b�V����
        //    if (energy > 0)
        //        energy -= SubDash;
        //}
        //else 
        if (!player)
            return;

        if (player.GetComponent<Player>().isStep)
        {// �X�e�b�v��
            if (!isStep)
            {
                if (energy > SubStep)
                {
                    isStep = true;
                    energy -= SubStep;
                }
            }
        }
        else if (player.GetComponent<Player>().isRoll)
        {// ���[����
            if (!isRoll)
            {
                if (energy > SubStep)
                {
                    isRoll = true;
                    energy -= SubRoll;
                }
            }
        }
        else
        {// �����ĂȂ�or�ʏ�ړ�����
            if (energy < maxEnergy)
                energy += SubDash;
        }



        //-UI�̍X�V
        // ���݂̃G�l���M�[���ő�G�l���M�[�Ŋ������䗦���v�Z
        energy_ratio = energy / maxEnergy;
        // �G�l���M�[�Q�[�W�̕\�����X�V
        image_Energygauge.fillAmount = energy_ratio;
    }
}
