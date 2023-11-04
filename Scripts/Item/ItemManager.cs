using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

//使い方
//１、ItemBaseを継承したクラス作る
//２、そのクラスでフィールドに置く用のPrefab作る
//３、そのPrefabをItemManagerPrefabの子オブジェクトにする
//４、ItemManagerPrefabを更新する


/// <summary>
/// アイテム管理用のシングルトンクラス
/// </summary>
public class ItemManager : MonoBehaviour, IManager
{
    #region　 Singleton
    static private ItemManager _instance;
    static public ItemManager Instance => _instance;
    #endregion

    [Tooltip("全てのアイテムとその所字数を格納するDictionary")]
    private List<ItemBase> _itemInventory = new ();
    public List<ItemBase> ItemInventory => _itemInventory;

    private bool _isAction = false;
    public bool IsAction => _isAction;

    private ItemType _actionEventItemType;

    private EventData _actionEventItemData;

    public delegate void DialogEvent(ItemData data);
    DialogEvent _showDialogEvent;

    public delegate void GetMessageEvent(ItemData data);
    GetMessageEvent _showMessageEvent;

    ReactiveProperty<bool> _namePlateSetEvent = new ReactiveProperty<bool>();

    public DialogEvent ShowDialogEvent
    {
        set { _showDialogEvent = value; }
    }

    public GetMessageEvent ShowMessageEvent
    {
        set { _showMessageEvent = value; }
    }

    public ReactiveProperty<bool> NamePlateSetEvent { get => _namePlateSetEvent; set => _namePlateSetEvent = value; }


    #region Initialize
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        AddChildrenItems();
    }

    /// <summary>
    /// 子オブジェクトになっているアイテムでDictionaryを初期化する
    /// </summary>
    private void AddChildrenItems()
    {
        ItemBase[] items = transform.GetComponentsInChildren<ItemBase>(true);
        foreach(ItemBase item in items)
        {
            _itemInventory.Add(item);
        }
    }
    #endregion

    #region Public Method

    private ItemBase itemBase;

    /// <summary>
    /// アイテムの個数を追加する
    /// </summary>
    /// <param name="item"></param>
    public ItemBase AddItem(ItemType itemType, bool isActionEvent = false, EventData eventData = null)
    {
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Get_Item");

        if (isActionEvent)
        {
            _isAction = true;
            _actionEventItemType = itemType;
            _actionEventItemData = eventData;
        }
        for(int i = 0; i < _itemInventory.Count; i++)
        {
            if (_itemInventory[i].ItemData.ItemType == itemType)
            {
                itemBase = _itemInventory[i];

                if(itemBase.IsFirstTimeGetting)
                {
                    _namePlateSetEvent.Value = true;
                    _showDialogEvent?.Invoke(itemBase.ItemData);
                }
                else
                {
                    _showMessageEvent?.Invoke(itemBase.ItemData);
                }

                itemBase.Add();
                return itemBase;
            }
        }
        Debug.LogError("該当するItemTypeがありません");
        return null;
    }

    /// <summary>
    /// アイテムを使用する
    /// </summary>
    public void UseItem(ItemType ItemType)
    {
        itemBase = _itemInventory.Find(m => m.ItemData.ItemType == ItemType);
        if (itemBase.Quantity == 0)
        {
            Debug.LogError("アイテムが0個です");
            return;
        }

        itemBase.UseEffect();
    }

    /// <summary>
    /// アイテムを捨てる
    /// </summary>
    public void DisposeItem(ItemType ItemType)
    {
        itemBase = _itemInventory.Find(m => m.ItemData.ItemType == ItemType);
        if (itemBase.Quantity == 0)
        {
            Debug.LogError("アイテムが0個です");
            return;
        }

        itemBase.Dispose();
    }

    /// <summary>
    /// アイテム取得後にイベントを走らせる
    /// </summary>
    public void ActionEvent()
    {
        _isAction = false;

        EventManager.Instance.EventStart(_actionEventItemData);
    }

    public void DestroyObject()
    {
        _instance = null;
        Destroy(this.gameObject);
    }
    #endregion
}
