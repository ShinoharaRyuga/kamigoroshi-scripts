using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using System;

/// <summary>�v���C���[�̃p�����[�^���Ǘ��E���삷��N���X </summary>
public class PlayerParamController : MonoBehaviour
{
    [SerializeField, Header("�p�����[�^�[�f�[�^")]
    PlayerParameter _playerParam = default;

    float _moveSpeed = 0f;
    ReactiveProperty<float> _hp = new ReactiveProperty<float>();
    ReactiveProperty<float> _attackPower = new ReactiveProperty<float>();

    #region �v���p�e�B
    public float MoveSpeed => _moveSpeed;

    /// <summary>�p�����[�^�[�f�[�^ </summary>
    public PlayerParameter ParamData => _playerParam;

    public ReactiveProperty<float> HP => _hp;

    public ReactiveProperty<float> AttackPower => _attackPower;
    #endregion

    /// <summary>�_���[�W�v�Z </summary>
    /// <param name="damageValue">�_���[�W�l</param>
    public void GetDamage(float damageValue, bool isInvincible)
    {
        if(isInvincible)
        {
            return;
        }
        else
        {
            _hp.Value = Mathf.Max(0, _hp.Value - damageValue);
        }
    }

    /// <summary>�p�����[�^�[������������ </summary>
    public void InitializeParameter(Action deadAction)
    {
        _hp.Value = _playerParam.MaxHP;
        _attackPower.Value = _playerParam.FirstAttackPower;
        _moveSpeed = _playerParam.MoveSpeed;

        _hp.Subscribe(x =>
        {
            if (_hp.Value <= 0)
            {
                deadAction.Invoke();
            }
        }).AddTo(this);
    }

    /// <summary>�A�C�e�����ʂ��������� ��������</summary>
    /// <param name="targetParam">���삷��p�����[�^�[</param>
    /// <param name="value">�����l</param>
    public void ItamEffectActive(ParameterType targetParam, float value)
    {
        switch (targetParam)
        {
            case ParameterType.HP:
                _hp.Value = (0 < value) ? Mathf.Min(_playerParam.MaxHP, _hp.Value + value) : Mathf.Max(0, _hp.Value - value);
                break;
            default:
                Debug.LogError("���̃p�����[�^�[�𑀍삷�鏈����������Ă��܂���");
                break;
        }
    }

    /// <summary>�A�C�e�����ʂ��������� ��������</summary>
    /// <param name="targetParam">���삷��p�����[�^�[</param>
    /// <param name="duration">��������</param>
    /// <param name="addValue">�����l</param>
    public async void ItemEffectHold(ParameterType targetParam, float duration, float addValue)
    {
        switch (targetParam)
        {
            case ParameterType.Attack:
                var ct = new CancellationToken();
                await ExecuteParamHold(_attackPower, duration, addValue, ct);
                break;
            default:
                Debug.LogError("���̃p�����[�^�[�𑀍삷�鏈����������Ă��܂���");
                break;
        }
    }

    /// <summary>�����A�C�e���̌��ʏ������s�����\�b�h </summary>
    /// <param name="targetParam">�Ώۂ̃p�����[�^�[</param>
    /// <param name="duration">��������</param>
    /// <param name="addValue">�����l</param>
    /// <param name="token">�L�����Z���g�[�N��</param>
    private async UniTask ExecuteParamHold(ReactiveProperty<float> targetParam, float duration, float addValue, CancellationToken token)
    {
        var tmp = targetParam.Value;
        targetParam.Value += addValue;

        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        targetParam.Value = tmp;
    }
}
