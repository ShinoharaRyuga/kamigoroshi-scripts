using System.Text;
using UnityEngine;

/// <summary>�v���C���[������������G�t�F�N�g���Ǘ�����N���X</summary>
public class PlayerEffectController : MonoBehaviour
{
    [SerializeField, Header("�a���G�t�F�N�g"), ElementNames(new string[] {"�R���{1", "�R���{2", "�R���{3"})]
    ParticleSystem[] _slashingEffects = default;
    [SerializeField, Header("�����G�t�F�N�g"), Tooltip("�ړ����g�p")]
    ParticleSystem _smokeEffect = default;
    [SerializeField, Header("�q�b�g�G�t�F�N�g")]
    ParticleSystem _hitEffect = default;

    private void Start()
    {
        foreach (var effect in _slashingEffects)
        {
            effect.Stop();
        }
    }

    /// <summary>�A�j���[�V�����g���K�[�ŌĂяo�� </summary>
    public void Attack01()
    {
        _slashingEffects[0].Play();
    }

    /// <summary>�A�j���[�V�����g���K�[�ŌĂяo�� </summary>
    public void Attack02()
    {
        _slashingEffects[1].Play();
    }

       /// <summary>�A�j���[�V�����g���K�[�ŌĂяo�� </summary>
    public void Attack03()
    {
        _slashingEffects[2].Play();
    }

     /// <summary>�ړ����ɍ����G�t�F�N�g���Đ����� </summary>
    public void PlaySmokeEffect()
    {
        if (_smokeEffect.isPlaying) { return; }

        _smokeEffect.Play();
    }

    /// <summary>�q�b�g�G�t�F�N�g���Đ� </summary>
    public void PlayHitEffect(Vector3 effectPos)
    {
        _hitEffect.transform.position = effectPos;
        _hitEffect.Play();
    }
}
