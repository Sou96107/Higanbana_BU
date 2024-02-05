using System.Collections;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private void Update()
    {
        // �X�y�[�X�L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // "TargetSceneName" �̕�����J�ڂ������V�[���̖��O�ɒu��������
            string targetSceneName = "SampleScene";

            // Fade �I�u�W�F�N�g�����邩�m�F���A���݂���΃t�F�[�h���������s
            Fade fade = FindObjectOfType<Fade>();
            if (fade != null)
            {
                fade.FadeToScene(targetSceneName);
            }
            else
            {
                // Fade �I�u�W�F�N�g��������Ȃ��ꍇ�A���ڃV�[�������[�h
                UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}