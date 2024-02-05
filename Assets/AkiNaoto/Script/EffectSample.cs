/*
 �����ӁI���������G�t�F�N�g�̑f�ނ�Unity�ɓ����ۂɂ͊K�w�\���ɒ��ӂ��܂��傤�B
 �ȉ��̃T�C�g�̈�ԉ��ɐ������ڂ��Ă܂����A�Ƃ肠������Assets/Resources�̒���
 �G�t�F�N�g�t�@�C�����A�e�N�X�`���͂���ɂ��̒���Texture�t�@�C���ɒ��֓�����OK
 https://note.com/hirokichi0623/n/n9beb5b093033
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class EffectSample : MonoBehaviour
{
    public bool bPlayEffect;//�����ɓ��ꂽ�G�t�F�N�g�g�p�۔���ϐ�

    // Start is called before the first frame update
    void Start()
    {
        if(bPlayEffect)
        {
            PlayEffect("Laser01");

            PlayEffect("Simple_Ring_Shape2");
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===================================================================================
    //�G�t�F�N�g�Đ��֐�
    //�������F�Đ��������G�t�F�N�g�̖��O
    //===================================================================================
    public void PlayEffect(string EffectName)
    {
        // �G�t�F�N�g���擾����B
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(EffectName);
        // transform�̈ʒu�ŃG�t�F�N�g���Đ�����
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, transform.position);
        // transform�̉�]��ݒ肷��B
        handle.SetRotation(transform.rotation);
    }

    public void PlayEffect(string EffectName, Vector3 pos)
    {
        // �G�t�F�N�g���擾����B
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(EffectName);
        // transform�̈ʒu�ŃG�t�F�N�g���Đ�����
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, pos);
        // transform�̉�]��ݒ肷��B
        handle.SetRotation(transform.rotation);
    }
}
