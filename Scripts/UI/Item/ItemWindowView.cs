using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemWindowView : SelectionWindowView, IUIResetView
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _descriptionText;
    [SerializeField] Image _itemSprite;
    [Space(10)]
    [SerializeField] RectTransform _nonItemText;

    ItemButtonView[] _itemButtons;
    ItemType? _itemType = null;

    protected override void OnAwake()
    {
        _itemButtons = new ItemButtonView[_children.Length];

        for(int i = 0; i < _children.Length; i++)
        {
            if (_children[i].TryGetComponent(out ItemButtonView button))
            {
                _itemButtons[i] = button;
                button.OnSelectEvent += SetText;
            }
        }

        ResetView();
    }

    void SetText(ItemBase item)
    {
        if(!item)
        {
            ResetView();
            return;
        }

        _itemType = item.ItemData.ItemType;
        _itemSprite.sprite = item.ItemData.ItemSprite;
        _nameText.text = item.ItemData.ItemName;
        _descriptionText.text = item.ItemData.ItemText;
    }

    public void ResetView()
    {
        _itemType = null;
        _nameText.text = string.Empty;
        _descriptionText.text = string.Empty;
    }

    /// <summary>
    /// 収集品の画面に行くボタンと、アイテムを使う確認ボタンにセット
    /// </summary>
    public void OnUpdateView()
    {
        var items = ItemManager.Instance.ItemInventory;
        int index = 0;
        int nextIndex = 0;

        for(int i = 0; i < _itemButtons.Length; i++)
        {
            ItemBase item = null;

            if(items.Count > i)
            {
                item = items[i];
            }

            _itemButtons[index].OnUpdateButton(item);

            if (item && item.Quantity >= 1)
            {
                Debug.Log($"Index = {index} : Item = {item.name}");

                _itemButtons[index].Setup(ChangeSelection, _nextWindows[nextIndex]);

                if (nextIndex < _nextWindows.Length - 1)
                {
                    nextIndex++;
                }

                index++;
            }
        }

        var flag = false;

        if (index == 0)
        {
            //アイテムを表示しない場合の処理
            flag = true;
            UIManager.Instance.SwitchToPreviousWindow();
        }

        _nonItemText.gameObject.SetActive(flag);
    }

    public void OnUseItem()
    {
        ItemManager.Instance.UseItem(_itemType.Value);
        OnUpdateView();

        UIManager.Instance.SwitchToPreviousWindow();
    }

    public void OnDisposeItem()
    {
        ItemManager.Instance.DisposeItem(_itemType.Value);
        OnUpdateView();

        UIManager.Instance.SwitchToPreviousWindow();
    }
}
