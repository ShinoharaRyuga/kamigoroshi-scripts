using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ上のアイテムの位置を表示するアイテム
/// </summary>
public class SearchItem : ItemBase
{
    [Header("パラメーター")]
    [SerializeField, Tooltip("効果持続時間")]
    public float _duration = 30f;
    [SerializeField]
    private GameObject _itemsUI;

    public override async void UseEffect()
    {
        Debug.Log("アイテム発見");
        base.UseEffect();


        //GameObjectをオンオフする
        //楽だが要改良
        _itemsUI?.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(_duration));
        _itemsUI?.SetActive(false);
    }
}
