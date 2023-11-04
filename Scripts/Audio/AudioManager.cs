using System.Collections.Generic;
using UnityEngine;
using CriWare;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.InputSystem;

/// <summary>サウンド関連を管理するクラス　シングルトンパターンを使用</summary>
public class AudioManager : MonoBehaviour
{
    static AudioManager _instance = default;

    [SerializeField, Header("フェードにかかる時間")]
    float _fadeTime = 1.0f;
    [SerializeField, Header("サウンドソースの親オブジェクト"), Tooltip("0=BGM, 1=SE, 2=VOICE")]
    [ElementNames(new string[] { "BGM", "SE", "VOICE" })]
    GameObject[] _sourceParents = default;
    [Space(10)]
    [SerializeField] int _volumeScale = 10;

    /// <summary>BGM音量 </summary>
    float _bgmVolumeScale;
    /// <summary>SE音量 </summary>
    float _seVolumeScale;
    /// <summary>VOICE音量 </summary>
    float _voiceVolumeScale;

    /// <summary>
    /// 各サウンド用のAtomSource配列
    /// <para>0=BGM用 1=SE用 2=VOICE用</para>
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

    /// <summary>サウンドを再生する </summary>
    /// <param name="soundType">再生するサウンドの種類</param>
    /// <param name="cueName">再生するcueName</param>
    /// <param name="volume">音量</param>
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

    /// <summary>再生中のサウンドを停止する</summary>
    /// <param name="cueName">停止するcueName</param>
    public void StopSound(SoundType type, string cueName)
    {
        var stopSource = GetStopSource(type, cueName);

        if (stopSource is not null)
        {
            stopSource.Stop();
        }
    }

    /// <summary>音量調整を行う為にリストに追加する </summary>
    /// <param name="type">サウンドタイプ</param>
    /// <param name="source">対象</param>
    public void AddAtomSource(SoundType type, CriAtomSource source, float volume)
    {
        if (!_criAtomSources[(int)type].ContainsKey(source))
        {
            _criAtomSources[(int)type].Add(source, volume);
        }
    }

    /// <summary>リストから破棄 </summary>
    /// <param name="type">サウンドタイプ</param>
    /// <param name="source">対象</param>
    public void RemoveAtomSource(SoundType type, CriAtomSource source)
    {
        if (_criAtomSources[(int)type].ContainsKey(source))
        {
            _criAtomSources[(int)type].Remove(source);
        }
    }

    /// <summary>音量を徐々に小さくする </summary>
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

    /// <summary>音量を徐々に大きくする </summary>
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

    /// <summary>bgmSourceの音量を変更する</summary>
    public void SetBgmVolume(float value)
    {
        _bgmVolumeScale = value;

        foreach (var source in _criAtomSources[(int)SoundType.BGM])
        {
            source.Key.volume = source.Value * (_bgmVolumeScale / _volumeScale);
        }
    }

    /// <summary>seSourceの音量を変更する</summary>
    public void SetSeVolume(float value)
    {
        _seVolumeScale = value;

        foreach (var source in _criAtomSources[(int)SoundType.SE])
        {
            source.Key.volume = source.Value * (_seVolumeScale / _volumeScale);
        }
    }

    /// <summary>voiceSourceの音量を変更する</summary>
    public void SetVoiceVolume(float value)
    {
        _voiceVolumeScale = value;

        foreach (var source in _criAtomSources[(int)SoundType.VOICE])
        {
            source.Key.volume = source.Value * (_voiceVolumeScale / _volumeScale);
        }
    }

    /// <summary>再生可能なsourceを取得する </summary>
    private CriAtomSource GetPlaySource(SoundType type)
    {
        foreach (var source in _criAtomSources[(int)type])
        {
            if (source.Key.status == CriAtomSourceBase.Status.PlayEnd || source.Key.status == CriAtomSourceBase.Status.Stop)
            {
                return source.Key;
            }
        }

        //再生可能なsourceがなければ新しく作成する
        var newSource = _sourceParents[(int)type].AddComponent<CriAtomSource>();
        return newSource;
    }

    /// <summary>再生中のサウンドを止める処理 </summary>
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

    /// <summary>各サウンド音量を計算する </summary>
    /// <param name="type">サウンド種類</param>
    /// <param name="soundVolume">音量</param>
    /// <returns>再生音量</returns>
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

    /// <summary>初期化処理 </summary>
    private void Initialize()
    {
        _bgmVolumeScale = _volumeScale / 2;
        _seVolumeScale = _volumeScale / 2;
        _voiceVolumeScale = _volumeScale / 2;
    }
}

