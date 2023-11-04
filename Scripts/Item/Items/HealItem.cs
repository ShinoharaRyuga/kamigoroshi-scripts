using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �񕜃A�C�e���p�̃N���X�@ItemBase���p�����Ă���
/// </summary>
public class HealItem : ItemBase
{
    [Header("�p�����[�^�[")]
    [SerializeField, Tooltip("�񕜂���l")]
    public int _healValue = 30;

    public override void UseEffect()
    {
        Debug.Log("��");
        base.UseEffect();

        GameManager.Instance.PlayerManager.ItemParamChangeActive(ParameterType.HP, _healValue);
    }
}
