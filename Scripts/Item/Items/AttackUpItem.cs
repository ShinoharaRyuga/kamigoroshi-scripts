using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃力アップアイテム用のクラス　ItemBaseを継承している
/// </summary>
public class AttackUpItem : ItemBase
{
    [Header("パラメーター")]
    [SerializeField, Tooltip("攻撃力アップの値")]
    public int _attackUpValue = 10;
    [SerializeField, Tooltip("効果持続時間")]
    public float _duration = 30f;

    public override void UseEffect()
    {
        Debug.Log("攻撃力アップ");
        base.UseEffect();

       // GameManager.Instance.Player.ItemParamChangeHold(ParameterType.Attack, _duration, _attackUpValue);
    }
}
