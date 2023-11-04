using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParameter", menuName = "ScriptableObjects/Parameter/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [SerializeField, Header("移動速度"), Min(1f)]
    float _moveSpeed = 1f;

    [SerializeField, Header("最大HP")]
    int _maxHP = 0;

    [SerializeField, Header("初期攻撃力")]
    int _firstAttackPower = 0;

    [SerializeField, Header("最大コンボ数")]
    int _maxComboCount = 0;

    [Tooltip("回避行動用パラメータ")]

    [SerializeField, Header("回避速度")]
    float _dodgeSpeed = 0f;

    [SerializeField, Header("無敵時間")]
    float _invincibleTime = 0f;

    [SerializeField, Header("回避速度補正値")]
    float _velocityCorrectionValue = 0f;

    #region アクセス
    /// <summary>移動速度</summary>
    public float MoveSpeed => _moveSpeed;

    /// <summary>最大HP</summary>
    public int MaxHP => _maxHP;

    /// <summary>初期攻撃力</summary>
    public int FirstAttackPower => _firstAttackPower;

    /// <summary>最大コンボ数 </summary>
    public int MaxComboCount => _maxComboCount;

    /// <summary>回避距離</summary>
    public float DodgeSpeed => _dodgeSpeed;

    /// <summary>無敵時間</summary>
    public float InvincibleTime => _invincibleTime;

    /// <summary>回避速度補正値</summary>
    public float VelocityCorrectionValue => _velocityCorrectionValue;
    #endregion
}
