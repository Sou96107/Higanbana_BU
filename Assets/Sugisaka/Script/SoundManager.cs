using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BGM
{
    [Tooltip("BGM�̖��O")]
    public string name;
    [Tooltip("BGM�̃t�@�C��")]
    public AudioClip clip;
    [Tooltip("����")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Tooltip("���[�v")]
    public bool loop = true;
    [HideInInspector]
    public AudioSource audiosource;
}
[System.Serializable]
public class SE
{
    [Tooltip("SE�̖��O")]
    public string name;
    [Tooltip("SE�̃t�@�C��")]
    public AudioClip clip;
    [Tooltip("����")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Tooltip("�s�b�`")]
    [Range(-3f, 3f)]
    public float pitch = 1f;
    [Tooltip("���[�v")]
    public bool loop = false;
    [HideInInspector]
    public AudioSource audiosource;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    [Header("BGM")]
    public BGM[] bgm;

    [SerializeField]
    [Header("SE")]
    public SE[] se;

    // ���ʏ����l
    //public float BGM_volume = 0.5f;
    //public float SE_volume = 0.5f;

    private string beforeSceneName;

    // �V���O���g����
    public static SoundManager instance;

    private void Awake()
    {
        // SoundManager�C���X�^���X�����݂��Ȃ���ΐ���
        // ���݂����Destroy�Creturn
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        // AudioSource�R���|�[�l���g��ǉ�
        foreach (BGM bgm in bgm)
        {
            bgm.audiosource = gameObject.AddComponent<AudioSource>();
            bgm.audiosource.clip = bgm.clip;
            bgm.audiosource.volume = bgm.volume;
            bgm.audiosource.loop = bgm.loop;
        }
        foreach (SE se in se)
        {
            se.audiosource = gameObject.AddComponent<AudioSource>();
            se.audiosource.clip = se.clip;
            se.audiosource.volume = se.volume;
            se.audiosource.pitch = se.pitch;
            se.audiosource.loop = se.loop;
        }
    }

    private void Start()
    {
        // ���݂̃V�[�������擾
        beforeSceneName = SceneManager.GetActiveScene().name;

        // BGM�Đ�
        if (beforeSceneName == "StartEvent")
            PlayBGM("Game");
        else 
            PlayBGM(beforeSceneName);

        // �V�[���؂�ւ����ɌĂяo�����\�b�h��o�^
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        if (beforeSceneName == "StartEvent")
        {
            beforeSceneName = "Game";
            return;
        }
        if (nextScene.name == "StartEvent")
        {
            StopBGM(beforeSceneName);
            PlayBGM("Game");
            beforeSceneName = nextScene.name;
            return;
        }

        StopBGM(beforeSceneName);
        PlayBGM(nextScene.name);
        beforeSceneName = nextScene.name;
    }

    // BGM�Đ�
    public void PlayBGM(string name)
    {
        foreach (BGM bgm in bgm)
        {
            if (bgm.name == name)
            {
                bgm.audiosource.Play();
                return;
            }
        }
    }
    // BGM��~
    public void StopBGM(string name)
    {
        foreach (BGM bgm in bgm)
        {
            if (bgm.name == name)
            {
                bgm.audiosource.Stop();
                return;
            }
        }
    }

    // SE�Đ�
    public void PlaySE(string name)
    {
        SE s = Array.Find(se, sound => sound.name == name);
        if (s == null)
        {
            print("Sound" + name + "was not found");
            return;
        }
        s.audiosource.volume = s.volume;
        s.audiosource.pitch = s.pitch;
        s.audiosource.PlayOneShot(s.clip);
    }
    // SE��~
    public void StopSE(string name)
    {
        SE s = Array.Find(se, sound => sound.name == name);
        if (s == null)
        {
            print("Sound" + name + "was not found");
            return;
        }
        s.audiosource.Stop();
    }
}
