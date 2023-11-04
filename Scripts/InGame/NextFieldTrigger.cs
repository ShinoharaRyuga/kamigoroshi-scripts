using UnityEngine;
using System;
using System.Linq;

/// <summary>���t�B�[���h�ɑJ�ڏ��������s����N���X </summary>
public class NextFieldTrigger : MonoBehaviour
{
    [SerializeField] private string[] _bgmCueName;
    [SerializeField] private string[] _seCueName;
    /// <summary>�J�ڏ��� </summary>
    event Action _onChangeScene = default;

    /// <summary>�J�ڏ��� </summary>
    public event Action OnChangeScene
    {
        add { _onChangeScene += value; }
        remove { _onChangeScene -= value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _onChangeScene?.Invoke();
            StopAudio();
        }
    }

    private void StopAudio()
    {
        _bgmCueName.ToList().ForEach(cueName => AudioManager.Instance.StopSound(SoundType.BGM, cueName));
        _seCueName.ToList().ForEach(cueName => AudioManager.Instance.StopSound(SoundType.SE, cueName));
    }
}
