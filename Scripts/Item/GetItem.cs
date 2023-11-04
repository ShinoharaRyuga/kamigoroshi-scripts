using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// アイテムを入手するクラス　プレイヤーに子要素としてアタッチする想定
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public partial class GetItem : TargetChecker
{
    [Tooltip("範囲内に入ったFieldItemクラスを保持する用")]
    private List<FieldItem> _fieldItemList = new();

    private ItemBase _itemBase;
    Transform _thisTransform;

    public FieldItem FieldItem => _fieldItemList.Count > 0 ? _fieldItemList[0] : null;

    private void Awake()
    {
        TryGetComponent(out _thisTransform);
    }

    //アイテムが入手可能範囲に入ったことを検知する
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out FieldItem fieldItem)) return;

        _fieldItemList.Add(fieldItem);

        if (!EventManager.Instance.IsPlaying)
        {
            fieldItem.Enter();
        }

        if (_fieldItemList.Count > 0)
        {
            _isEnable.Value = true;
        }
    }

    //アイテムが入手可能範囲から出たことを検知する
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out FieldItem fieldItem)) return;

        _fieldItemList.Remove(fieldItem);
        fieldItem.Exit();

        if (_fieldItemList.Count <= 0)
        {
            _isEnable.Value = false;
        }
    }

    public override bool OnCheck()
    {
        return false;
    }

    /// <summary>
    /// 範囲内にあるアイテムの関数を呼ぶ
    /// </summary>
    public override void OnFuncCall()
    {
        if (_fieldItemList.Count <= 0) return;

        FieldItem target = default;
        var minDis = 100f;      //取り合えず大きめな値を入れておく

        //近いほうを対象にする
        foreach (var item in _fieldItemList)
        {
            var dis = Vector2.Distance(_thisTransform.position, item.transform.position);

            if (dis >= minDis) continue;

            minDis = dis;
            target = item;
        }

        _fieldItemList.Remove(target);

        if (_fieldItemList.Count <= 0)
        {
            _isEnable.Value = false;
        }

        //ItemManagerにアイテムを追加
        _itemBase = ItemManager.Instance.AddItem(target.Get(), target.GetBool(), target.GetEventData());
    }
}