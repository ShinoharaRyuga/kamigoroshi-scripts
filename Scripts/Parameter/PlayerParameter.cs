using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParameter", menuName = "ScriptableObjects/Parameter/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [SerializeField, Header("�ړ����x"), Min(1f)]
    float _moveSpeed = 1f;

    [SerializeField, Header("�ő�HP")]
    int _maxHP = 0;

    [SerializeField, Header("�����U����")]
    int _firstAttackPower = 0;

    [SerializeField, Header("�ő�R���{��")]
    int _maxComboCount = 0;

    [Tooltip("����s���p�p�����[�^")]

    [SerializeField, Header("��𑬓x")]
    float _dodgeSpeed = 0f;

    [SerializeField, Header("���G����")]
    float _invincibleTime = 0f;

    [SerializeField, Header("��𑬓x�␳�l")]
    float _velocityCorrectionValue = 0f;

    #region �A�N�Z�X
    /// <summary>�ړ����x</summary>
    public float MoveSpeed => _moveSpeed;

    /// <summary>�ő�HP</summary>
    public int MaxHP => _maxHP;

    /// <summary>�����U����</summary>
    public int FirstAttackPower => _firstAttackPower;

    /// <summary>�ő�R���{�� </summary>
    public int MaxComboCount => _maxComboCount;

    /// <summary>�������</summary>
    public float DodgeSpeed => _dodgeSpeed;

    /// <summary>���G����</summary>
    public float InvincibleTime => _invincibleTime;

    /// <summary>��𑬓x�␳�l</summary>
    public float VelocityCorrectionValue => _velocityCorrectionValue;
    #endregion
}
