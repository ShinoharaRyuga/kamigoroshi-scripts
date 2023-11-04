using CriWare;
using UnityEngine;

/// <summary>�G�̃T�E���h���Đ�����N���X</summary>
public class EnemySoundPlayer : MonoBehaviour
{
    [SerializeField, Header("AtomSource")] CriAtomSource _criAtomSource = default;

    [SerializeField, Header("�U���T�E���h��cueName")]
    string _attackCueName = "";

    [SerializeField, Header("�_���[�W�T�E���h��cueName")]
    string _damageCueName = "";

    private void Start()
    {
        AudioManager.Instance.AddAtomSource(SoundType.VOICE, _criAtomSource, _criAtomSource.volume);
    }

    /// <summary>�U���T�E���h���Đ� </summary>
    public void PlayAttackSound()
    {
        _criAtomSource.Play(_attackCueName);
    }

    /// <summary>�_���[�W�T�E���h���Đ� </summary>
    public void PlayDamageSound()
    {
        _criAtomSource.Play(_damageCueName);
    }

    /// <summary>���S������ </summary>
    public void Dead()
    {
        AudioManager.Instance.RemoveAtomSource(SoundType.VOICE, _criAtomSource);

        AudioManager.Instance.StopSound(SoundType.BGM, "BGM_Battle");
        AudioManager.Instance.StopSound(SoundType.SE, "SE_HeartBeat");
    }
}