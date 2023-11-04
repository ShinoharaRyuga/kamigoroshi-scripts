using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

//Enter()とExit()は証拠品とかと共通化できそう
//Interact()はIInteractedインターフェイスとか作って抽象化できそう


//[TODO] 「□で入手する」みたいなUIを表示できるようにする

/// <summary>
/// フィールドにあるアイテムにアタッチする
/// プレイヤーのGetItemクラスから呼び出される
/// </summary>
public class FieldItem : MonoBehaviour
{
    [SerializeField, Tooltip("ItemManagerに渡すアイテムのData")]
    ItemData _itemData;
    [SerializeField]
    private FieldTargetNameView _fieldItemUi;

    [SerializeField] private Sprite _sprite;

    private void Start()
    {
        _fieldItemUi.Initialize(_sprite);
        _fieldItemUi.gameObject.SetActive(true);
    }

    public void Enter()
    {
        _fieldItemUi.OnShow();
    }

    public void Exit()
    {
        _fieldItemUi.OnHide();
    }

    /// <summary>
    /// アイテム取得範囲内で入手ボタンを押すと呼び出される
    /// </summary>
    public ItemType Get()
    {
        //とりあえず　後々とった時のアニメーションとかを入れる
        Destroy(gameObject);

        return _itemData.ItemType;
    }

    /// <summary>
    /// 取ったアイテムがイベントを発行するかどうか
    /// </summary>
    /// <returns></returns>
    public bool GetBool()
    {
        return _itemData.IsActionEventItem;
    }

    /// <summary>
    /// 取ったアイテムのイベントデータを取得
    /// </summary>
    /// <returns></returns>
    public EventData GetEventData()
    {
        return _itemData.EventData;
    }
}
