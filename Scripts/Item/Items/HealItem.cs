using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回復アイテム用のクラス　ItemBaseを継承している
/// </summary>
public class HealItem : ItemBase
{
    [Header("パラメーター")]
    [SerializeField, Tooltip("回復する値")]
    public int _healValue = 30;

    public override void UseEffect()
    {
        Debug.Log("回復");
        base.UseEffect();

        GameManager.Instance.PlayerManager.ItemParamChangeActive(ParameterType.HP, _healValue);
    }
}
