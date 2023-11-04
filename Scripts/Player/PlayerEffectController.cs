using System.Text;
using UnityEngine;

/// <summary>プレイヤーが発生させるエフェクトを管理するクラス</summary>
public class PlayerEffectController : MonoBehaviour
{
    [SerializeField, Header("斬撃エフェクト"), ElementNames(new string[] {"コンボ1", "コンボ2", "コンボ3"})]
    ParticleSystem[] _slashingEffects = default;
    [SerializeField, Header("砂埃エフェクト"), Tooltip("移動時使用")]
    ParticleSystem _smokeEffect = default;
    [SerializeField, Header("ヒットエフェクト")]
    ParticleSystem _hitEffect = default;

    private void Start()
    {
        foreach (var effect in _slashingEffects)
        {
            effect.Stop();
        }
    }

    /// <summary>アニメーショントリガーで呼び出し </summary>
    public void Attack01()
    {
        _slashingEffects[0].Play();
    }

    /// <summary>アニメーショントリガーで呼び出し </summary>
    public void Attack02()
    {
        _slashingEffects[1].Play();
    }

       /// <summary>アニメーショントリガーで呼び出し </summary>
    public void Attack03()
    {
        _slashingEffects[2].Play();
    }

     /// <summary>移動時に砂埃エフェクトを再生する </summary>
    public void PlaySmokeEffect()
    {
        if (_smokeEffect.isPlaying) { return; }

        _smokeEffect.Play();
    }

    /// <summary>ヒットエフェクトを再生 </summary>
    public void PlayHitEffect(Vector3 effectPos)
    {
        _hitEffect.transform.position = effectPos;
        _hitEffect.Play();
    }
}
