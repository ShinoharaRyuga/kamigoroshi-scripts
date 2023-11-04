using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの基底クラス
/// テキストデータや各種パラメーター、使用時の関数などを持つ
/// </summary>
public abstract class ItemBase : MonoBehaviour 
{
    [SerializeField, Tooltip("アイテムのデータ")]
    private ItemData _itemTextData;
    [SerializeField, Min(0)]
    private int _quantity = 0;
    [SerializeField]
    private bool _isFirstTimeGetting = true;

    public ItemData ItemData => _itemTextData; 
    public int Quantity => _quantity;
    public bool IsFirstTimeGetting => _isFirstTimeGetting;

    public virtual void UseEffect()
    {
        _quantity--;
    }

    public virtual void Add()
    {
        if(_isFirstTimeGetting)
        {
            _isFirstTimeGetting = false;
        }

        _quantity++;
    }

    public void Dispose()
    {
        _quantity--;
    }
}
