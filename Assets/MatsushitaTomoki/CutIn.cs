using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutIn : MonoBehaviour
{
    public float countdownTime = 60.0f; // カウントダウンする秒数

    [SerializeField] private float currentTime;

    private VideoPlayer videoPlayer;
    private float frameTime;

    private void Awake()
    {
        FrameManager.Add(gameObject, "CutIn");
    }

    private void Start()
    {
        currentTime = countdownTime;
        videoPlayer = GetComponent<VideoPlayer>();
        frameTime = 1.0f;
    }

    private void Update()
    {
        if (frameTime <= 0.0f)
        {
            videoPlayer.Pause();
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            // タイマーが0になったときの処理をここに追加する（例：ゲームオーバー処理など）
            ResetTimer();
            gameObject.SetActive(false);
            FrameManager.Remove(gameObject, "CutIn");
        }
    }


    public void ResetTimer()
    {
        currentTime = countdownTime;
    }

    public void StartFrame()
    {
        frameTime = 1.0f;
        videoPlayer.Play();
        Debug.Log("カットインフレーム：" + frameTime);
    }

    public void StopFrame()
    {
        frameTime = 0.0f;
        Debug.Log("カットインフレーム：" + frameTime);
    }
}
