using UnityEngine;
using UnityEngine.UI;

public class CutIn : MonoBehaviour
{
    public float countdownTime = 60.0f; // �J�E���g�_�E������b��

    [SerializeField] private float currentTime;

    private void Start()
    {
        currentTime = countdownTime;
    }

    private void Update()
    {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                // �^�C�}�[��0�ɂȂ����Ƃ��̏����������ɒǉ�����i��F�Q�[���I�[�o�[�����Ȃǁj
                ResetTimer();
                gameObject.SetActive(false);
            }
    }


    public void ResetTimer()
    {
        currentTime = countdownTime;
    }
}
