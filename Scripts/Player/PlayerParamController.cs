using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using System;

/// <summary>プレイヤーのパラメータを管理・操作するクラス </summary>
public class PlayerParamController : MonoBehaviour
{
    [SerializeField, Header("パラメーターデータ")]
    PlayerParameter _playerParam = default;

    float _moveSpeed = 0f;
    ReactiveProperty<float> _hp = new ReactiveProperty<float>();
    ReactiveProperty<float> _attackPower = new ReactiveProperty<float>();

    #region プロパティ
    public float MoveSpeed => _moveSpeed;

    /// <summary>パラメーターデータ </summary>
    public PlayerParameter ParamData => _playerParam;

    public ReactiveProperty<float> HP => _hp;

    public ReactiveProperty<float> AttackPower => _attackPower;
    #endregion

    /// <summary>ダメージ計算 </summary>
    /// <param name="damageValue">ダメージ値</param>
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

    /// <summary>パラメーターを初期化する </summary>
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

    /// <summary>アイテム効果を処理する 即時発動</summary>
    /// <param name="targetParam">操作するパラメーター</param>
    /// <param name="value">増減値</param>
    public void ItamEffectActive(ParameterType targetParam, float value)
    {
        switch (targetParam)
        {
            case ParameterType.HP:
                _hp.Value = (0 < value) ? Mathf.Min(_playerParam.MaxHP, _hp.Value + value) : Mathf.Max(0, _hp.Value - value);
                break;
            default:
                Debug.LogError("そのパラメーターを操作する処理が書かれていません");
                break;
        }
    }

    /// <summary>アイテム効果を処理する 持続効果</summary>
    /// <param name="targetParam">操作するパラメーター</param>
    /// <param name="duration">持続時間</param>
    /// <param name="addValue">増減値</param>
    public async void ItemEffectHold(ParameterType targetParam, float duration, float addValue)
    {
        switch (targetParam)
        {
            case ParameterType.Attack:
                var ct = new CancellationToken();
                await ExecuteParamHold(_attackPower, duration, addValue, ct);
                break;
            default:
                Debug.LogError("そのパラメーターを操作する処理が書かれていません");
                break;
        }
    }

    /// <summary>持続アイテムの効果処理を行うメソッド </summary>
    /// <param name="targetParam">対象のパラメーター</param>
    /// <param name="duration">持続時間</param>
    /// <param name="addValue">増減値</param>
    /// <param name="token">キャンセルトークン</param>
    private async UniTask ExecuteParamHold(ReactiveProperty<float> targetParam, float duration, float addValue, CancellationToken token)
    {
        var tmp = targetParam.Value;
        targetParam.Value += addValue;

        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        targetParam.Value = tmp;
    }
}
