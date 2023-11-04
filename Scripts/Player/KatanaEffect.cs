using UnityEngine;

/// <summary>刀のエフェクトを適用</summary>
public class KatanaEffect : MonoBehaviour
{
    [SerializeField, Header("エフェクトを適用するか")]
    bool _isEffect = true;

    [SerializeField]
    ParticleSystem _katanaEffect = default;

    [SerializeField]
    Material _katanaMat = default;

    [SerializeField]
    float _clipTime = default;

    void Start()
    {
        _katanaEffect.Stop();
        _katanaMat.SetFloat("_Flag", 0);
        _katanaMat.SetFloat("_ClipTime", 0);

        if (_isEffect)
        {
            Initialize();
        }
    }

    /// <summary>初期化処理 </summary>
    private void Initialize()
    {
        _katanaEffect.Play();
        _katanaMat.SetFloat("_Flag", 1);
        _katanaMat.SetFloat("_ClipTime", _clipTime);
    }
}
