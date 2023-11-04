using UnityEngine;

/// <summary>���̃G�t�F�N�g��K�p</summary>
public class KatanaEffect : MonoBehaviour
{
    [SerializeField, Header("�G�t�F�N�g��K�p���邩")]
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

    /// <summary>���������� </summary>
    private void Initialize()
    {
        _katanaEffect.Play();
        _katanaMat.SetFloat("_Flag", 1);
        _katanaMat.SetFloat("_ClipTime", _clipTime);
    }
}
