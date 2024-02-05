using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField, Header("�G")]
    private GameObject enemy;

    [SerializeField, Header("HPBar�̌���Image")]
    private Image image_Backgauge;

    // �v���C���[�̍ő�HP
    float maxHP = 100;

    // ���݂�HP�i0����100�͈̔͂Őݒ�j
    float hp = 100;

    // HSL�F��Ԃ̃p�����[�^
    [Range(0, 1)] public float color_h, color_s, color_l;

    // HP�Q�[�W��UI�C���[�W
    private Image image_HPgauge;

    // ���݂�HP���ő�HP�Ŋ������䗦
    private float hp_ratio;

    private float back_ratio;
    private float speed = 0.05f;
    private float acc = 0.0f;

    // �v���C���[�̃X�N���v�g
    private Enemy EnemyCS;
    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̃X�N���v�g�擾
        EnemyCS = enemy.GetComponent<Enemy>();
        maxHP = EnemyCS.GetMaxHp();
        Debug.Log(maxHP);
        // HP�Q�[�W��UI�C���[�W���擾
        image_HPgauge = gameObject.GetComponent<Image>();

        back_ratio = 1.0f;

        // �����̐F��S�i�ʓx�j��L�i���x�j��ݒ�
        color_s = 1.0f;
        color_l = 0.5f;

    }

    Color HSLtoRGB(float H, float S, float L)
    {
        Color col = new Color(1f, 1f, 1f, 1f);

        // �F���Ɋ�Â���RGB�F���v�Z
        float max = L + S * (1 - Mathf.Abs(2f * L - 1f)) * 0.5f;
        float min = L - S * (1 - Mathf.Abs(2f * L - 1f)) * 0.5f;

        int i = (int)Mathf.Floor(6.0f * H);

        switch (i)
        {
            case 0:
                //col.r = max;
                //col.g = min + (max - min) * 6.0f * H;
                //col.b = min;
                col.r = max;
                col.g = min;
                col.b = min + (max - min) * 6.0f * H;
                break;
            case 1:
                //col.r = min + (max - min) * (2.0f - 6.0f * H);
                //col.g = max;
                //col.b = min;
                col.r = min + (max - min) * (2.0f - 6.0f * H);
                col.g = min;
                col.b = max;
                break;
            case 2:
                //col.r = min;
                //col.g = max;
                //col.b = min + (max - min) * (6.0f * H - 2.0f);
                col.r = min;
                col.g = min + (max - min) * (6.0f * H - 2.0f);
                col.b = max;
                break;
            case 3:
                //col.r = min;
                //col.g = min + (max - min) * (4.0f - 6.0f * H);
                //col.b = max;
                col.r = min;
                col.g = max;
                col.b = min + (max - min) * (4.0f - 6.0f * H); ;
                break;
            case 4:
                //col.r = min + (max - min) * (6.0f * H - 4.0f);
                //col.g = min;
                //col.b = max;
                col.r = min + (max - min) * (6.0f * H - 4.0f);
                col.g = max;
                col.b = min;
                break;
            case 5:
                //col.r = max;
                //col.g = min;
                //col.b = min + (max - min) * (6.0f - 6.0f * H);
                col.r = max;
                col.g = min + (max - min) * (6.0f - 6.0f * H);
                col.b = min;
                break;
            default:
                col.r = max;
                col.g = max;
                col.b = max;
                break;
        }
        return col;
    }

    // Update is called once per frame
    void Update()
    {

        // �G��HP���擾
        hp = EnemyCS.GetHp();

        // ���݂�HP�̊������v�Z
        hp_ratio = hp / maxHP;

        // HP�Q�[�W��UI���X�V
        image_HPgauge.fillAmount = hp_ratio;

        // Hp��������
        if (hp_ratio <= back_ratio)
        {
            // �������������Ȃ���Q�[�W�����炷
            back_ratio -= speed + acc;
            image_Backgauge.fillAmount = back_ratio;
            acc += speed * Time.deltaTime;
        }
        else
            acc = 0.0f;

        // �F����HP�ɉ����ĕω�������
        color_h = Mathf.Lerp(0f, 0.4f, hp_ratio);

        // HP�Q�[�W�̐F��ݒ�
        image_HPgauge.color = HSLtoRGB(color_h, color_s, color_l);
    }
}
