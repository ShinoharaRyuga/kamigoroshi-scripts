using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���̓A�b�v�A�C�e���p�̃N���X�@ItemBase���p�����Ă���
/// </summary>
public class AttackUpItem : ItemBase
{
    [Header("�p�����[�^�[")]
    [SerializeField, Tooltip("�U���̓A�b�v�̒l")]
    public int _attackUpValue = 10;
    [SerializeField, Tooltip("���ʎ�������")]
    public float _duration = 30f;

    public override void UseEffect()
    {
        Debug.Log("�U���̓A�b�v");
        base.UseEffect();

       // GameManager.Instance.Player.ItemParamChangeHold(ParameterType.Attack, _duration, _attackUpValue);
    }
}
