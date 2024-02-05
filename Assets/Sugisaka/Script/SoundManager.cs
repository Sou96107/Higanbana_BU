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
    [Tooltip("BGMの名前")]
    public string name;
    [Tooltip("BGMのファイル")]
    public AudioClip clip;
    [Tooltip("音量")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Tooltip("ループ")]
    public bool loop = true;
    [HideInInspector]
    public AudioSource audiosource;
}
[System.Serializable]
public class SE
{
    [Tooltip("SEの名前")]
    public string name;
    [Tooltip("SEのファイル")]
    public AudioClip clip;
    [Tooltip("音量")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Tooltip("ピッチ")]
    [Range(-3f, 3f)]
    public float pitch = 1f;
    [Tooltip("ループ")]
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

    // 音量初期値
    //public float BGM_volume = 0.5f;
    //public float SE_volume = 0.5f;

    private string beforeSceneName;

    // シングルトン化
    public static SoundManager instance;

    private void Awake()
    {
        // SoundManagerインスタンスが存在しなければ生成
        // 存在すればDestroy，return
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

        // AudioSourceコンポーネントを追加
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
        // 現在のシーン名を取得
        beforeSceneName = SceneManager.GetActiveScene().name;

        // BGM再生
        if (beforeSceneName == "StartEvent")
            PlayBGM("Game");
        else 
            PlayBGM(beforeSceneName);

        // シーン切り替え時に呼び出すメソッドを登録
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

    // BGM再生
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
    // BGM停止
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

    // SE再生
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
    // SE停止
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
