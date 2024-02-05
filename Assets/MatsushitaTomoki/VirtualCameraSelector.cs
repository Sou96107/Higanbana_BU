using UnityEngine;
using Cinemachine;

/// <summary>
/// �o�[�`�����J������؂�ւ���T���v��
/// </summary>
public class VirtualCameraSelector : MonoBehaviour
{
    string name = "LButton"; //�o�^�������O

    // �o�[�`�����J�����ꗗ
    [SerializeField] private CinemachineVirtualCameraBase[] _virtualCameraList;

    // ��I�����̃o�[�`�����J�����̗D��x
    [SerializeField] private int _unselectedPriority = 0;

    // �I�����̃o�[�`�����J�����̗D��x
    [SerializeField] private int _selectedPriority = 10;

    // �I�𒆂̃o�[�`�����J�����̃C���f�b�N�X
    private int _currentCamera = 0;

    // �o�[�`�����J�����̗D��x������
    private void Awake()
    {
        // �o�[�`�����J�������ݒ肳��Ă��Ȃ���΁A�������Ȃ�
        if (_virtualCameraList == null || _virtualCameraList.Length <= 0)
            return;

        // �o�[�`�����J�����̗D��x��������
        for (var i = 0; i < _virtualCameraList.Length; ++i)
        {
            _virtualCameraList[i].Priority =
                (i == _currentCamera ? _selectedPriority : _unselectedPriority);
        }
    }

    // �t���[���X�V
    private void Update()
    {
        // �o�[�`�����J�������ݒ肳��Ă��Ȃ���΁A�������Ȃ�
        if (_virtualCameraList == null || _virtualCameraList.Length <= 0)
            return;

        // L�{�^���������ꂽ��
        if (Input.GetButtonDown(name))
        {
            // �ȑO�̃o�[�`�����J�������I��
            var vCamPrev = _virtualCameraList[_currentCamera];
            vCamPrev.Priority = _unselectedPriority;

            // �Ǐ]�Ώۂ����Ԃɐ؂�ւ�
            if (++_currentCamera >= _virtualCameraList.Length)
                _currentCamera = 0;

            // ���̃o�[�`�����J������I��
            var vCamCurrent = _virtualCameraList[_currentCamera];
            vCamCurrent.Priority = _selectedPriority;
        }
    }
}