using System.Collections.Generic;
using UnityEngine;
using CriWare;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.InputSystem;

/// <summary>�T�E���h�֘A���Ǘ�����N���X�@�V���O���g���p�^�[�����g�p</summary>
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance = default;

    [SerializeField, Header("�t�F�[�h�ɂ����鎞��")]
    float _fadeTime = 1.0f;
    [SerializeField, Header("�T�E���h�\�[�X�̐e�I�u�W�F�N�g"), Tooltip("0=BGM, 1=SE, 2=VOICE")]
    [ElementNames(new string[] { "BGM", "SE", "VOICE" })]
    GameObject[] _sourceParents = default;
    [Space(10)]
    [SerializeField] int _volumeScale = 10;

    /// <summary>BGM���� </summary>
    float _bgmVolumeScale;
    /// <summary>SE���� </summary>
    float _seVolumeScale;
    /// <summary>VOICE���� </summary>
    float _voiceVolumeScale;

    /// <summary>
    /// �e�T�E���h�p��AtomSource�z��
    /// <para>0=BGM�p 1=SE�p 2=VOICE�p</para>
    /// </summary>
    Dictionary<CriAtomSource, float>[] _criAtomSources = new Dictionary<CriAtomSource, float>[]
    {
         new Dictionary<CriAtomSource, float>(),
         new Dictionary<CriAtomSource, float>(),
         new Dictionary<CriAtomSource, float>(),
    };

    public static AudioManager Instance => _instance;
    public int VolumeScale => _volumeScale;
    public float BGMVolumeScale => _bgmVolumeScale;
    public float VoiceVolumeScale => _voiceVolumeScale;
    public float SEVolumeScale => _seVolumeScale;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;

            for (var i = 0; i < transform.childCount; i++)
            {
                _sourceParents[i] = transform.GetChild(i).gameObject;
            }

            Initialize();
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>�T�E���h���Đ����� </summary>
    /// <param name="soundType">�Đ�����T�E���h�̎��</param>
    /// <param name="cueName">�Đ�����cueName</param>
    /// <param name="volume">����</param>
    public void PlaySound(SoundType soundType, string cueName, float volume = 1f)
    {
        var playSource = GetPlaySource(soundType);
        playSource.volume = SetVolume(soundType, volume);
        playSource.cueName = cueName;

        playSource.Play();

        if (!_criAtomSources[(int)soundType].ContainsKey(playSource))
        {
            _criAtomSources[(int)soundType].Add(playSource, volume);
        }
    }

    /// <summary>�Đ����̃T�E���h���~����</summary>
    /// <param name="cueName">��~����cueName</param>
    public void StopSound(SoundType type, string cueName)
    {
        var stopSource = GetStopSource(type, cueName);

        if (stopSource is not null)
        {
            stopSource.Stop();
        }
    }

    /// <summary>���ʒ������s���ׂɃ��X�g�ɒǉ����� </summary>
    /// <param name="type">�T�E���h�^�C�v</param>
    /// <param name="source">�Ώ�</param>
    public void AddAtomSource(SoundType type, CriAtomSource source, float volume)
    {
        if (!_criAtomSources[(int)type].ContainsKey(source))
        {
            _criAtomSources[(int)type].Add(source, volume);
        }
    }

    /// <summary>���X�g����j�� </summary>
    /// <param name="type">�T�E���h�^�C�v</param>
    /// <param name="source">�Ώ�</param>
    public void RemoveAtomSource(SoundType type, CriAtomSource source)
    {
        if (_criAtomSources[(int)type].ContainsKey(source))
        {
            _criAtomSources[(int)type].Remove(source);
        }
    }

    /// <summary>���ʂ����X�ɏ��������� </summary>
    public void FadeOut()
    {
        for (var i = 0; i < _criAtomSources.Length; i++)
        {
            var sources = _criAtomSources[i];

            foreach (var source in sources)
            {
                DOVirtual.Float(source.Key.volume, 0f, _fadeTime, value =>
                {
                    source.Key.volume = value;
                });
            }
        }
    }

    /// <summary>���ʂ����X�ɑ傫������ </summary>
    public void FadeIn()
    {
        for (var i = 0; i < _criAtomSources.Length; i++)
        {
            var sources = _criAtomSources[i];
            var volume = 0f;

            foreach (var source in sources)
            {
                switch ((SoundType)i)
                {
                    case SoundType.BGM:
                        volume = source.Value * _bgmVolumeScale;
                        break;

                    case SoundType.SE:
                        volume = source.Value * _seVolumeScale;
                        break;

                    case SoundType.VOICE:
                        volume = source.Value * _voiceVolumeScale;
                        break;
                }

                DOVirtual.Float(source.Key.volume, volume / _volumeScale, _fadeTime, value =>
                {
                    source.Key.volume = value;
                });
            }
        }
    }

    /// <summary>bgmSource�̉��ʂ�ύX����</summary>
    public void SetBgmVolume(float value)
    {
        _bgmVolumeScale = value;

        foreach (var source in _criAtomSources[(int)SoundType.BGM])
        {
            source.Key.volume = source.Value * (_bgmVolumeScale / _volumeScale);
        }
    }

    /// <summary>seSource�̉��ʂ�ύX����</summary>
    public void SetSeVolume(float value)
    {
        _seVolumeScale = value;

        foreach (var source in _criAtomSources[(int)SoundType.SE])
        {
            source.Key.volume = source.Value * (_seVolumeScale / _volumeScale);
        }
    }

    /// <summary>voiceSource�̉��ʂ�ύX����</summary>
    public void SetVoiceVolume(float value)
    {
        _voiceVolumeScale = value;

        foreach (var source in _criAtomSources[(int)SoundType.VOICE])
        {
            source.Key.volume = source.Value * (_voiceVolumeScale / _volumeScale);
        }
    }

    /// <summary>�Đ��\��source���擾���� </summary>
    private CriAtomSource GetPlaySource(SoundType type)
    {
        foreach (var source in _criAtomSources[(int)type])
        {
            if (source.Key.status == CriAtomSourceBase.Status.PlayEnd || source.Key.status == CriAtomSourceBase.Status.Stop)
            {
                return source.Key;
            }
        }

        //�Đ��\��source���Ȃ���ΐV�����쐬����
        var newSource = _sourceParents[(int)type].AddComponent<CriAtomSource>();
        return newSource;
    }

    /// <summary>�Đ����̃T�E���h���~�߂鏈�� </summary>
    private CriAtomSource GetStopSource(SoundType type, string cueName)
    {
        foreach (var source in _criAtomSources[(int)type])
        {
            if (source.Key.status == CriAtomSourceBase.Status.Playing && source.Key.cueName == cueName)
            {
                return source.Key;
            }
        }

        return null;
    }

    /// <summary>�e�T�E���h���ʂ��v�Z���� </summary>
    /// <param name="type">�T�E���h���</param>
    /// <param name="soundVolume">����</param>
    /// <returns>�Đ�����</returns>
    private float SetVolume(SoundType type, float soundVolume)
    {
        switch (type)
        {
            case SoundType.BGM:
                return soundVolume * (_bgmVolumeScale / _volumeScale) * 1;
            case SoundType.SE:
                return soundVolume * (_seVolumeScale / _volumeScale) * 1;
            case SoundType.VOICE:
                return soundVolume * (_voiceVolumeScale / _volumeScale) * 1;
            default:
                return soundVolume;
        }
    }

    /// <summary>���������� </summary>
    private void Initialize()
    {
        _bgmVolumeScale = _volumeScale / 2;
        _seVolumeScale = _volumeScale / 2;
        _voiceVolumeScale = _volumeScale / 2;
    }
}

