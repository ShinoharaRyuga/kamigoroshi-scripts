using CriWare;
using UnityEngine;

/// <summary>敵のサウンドを再生するクラス</summary>
public class EnemySoundPlayer : MonoBehaviour
{
    [SerializeField, Header("AtomSource")] CriAtomSource _criAtomSource = default;

    [SerializeField, Header("攻撃サウンドのcueName")]
    string _attackCueName = "";

    [SerializeField, Header("ダメージサウンドのcueName")]
    string _damageCueName = "";

    private void Start()
    {
        AudioManager.Instance.AddAtomSource(SoundType.VOICE, _criAtomSource, _criAtomSource.volume);
    }

    /// <summary>攻撃サウンドを再生 </summary>
    public void PlayAttackSound()
    {
        _criAtomSource.Play(_attackCueName);
    }

    /// <summary>ダメージサウンドを再生 </summary>
    public void PlayDamageSound()
    {
        _criAtomSource.Play(_damageCueName);
    }

    /// <summary>死亡時処理 </summary>
    public void Dead()
    {
        AudioManager.Instance.RemoveAtomSource(SoundType.VOICE, _criAtomSource);

        AudioManager.Instance.StopSound(SoundType.BGM, "BGM_Battle");
        AudioManager.Instance.StopSound(SoundType.SE, "SE_HeartBeat");
    }
}