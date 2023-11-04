using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeaderButtonView : MonoBehaviour, ISelectHandler
{
    [SerializeField] TabType _type;
    [SerializeField] SelectionWindowView[] _selectionWindowView;

    Action _onClick;

    public TabType Type => _type;

    public enum TabType
    {
        Map,
        Mission,
        Item,
        Setting,
    }

    public void Setup(Action onClick, SelectionWindowView next)
    {
        if (TryGetComponent(out Button button))
        {
            button.onClick.RemoveAllListeners();
            _onClick = null;

            _onClick += ()=>
            {
                if(_type != TabType.Item)
                {
                    next.OnSwitchToValidWindow();
                    return;
                }

                var flag = false;

                foreach(var item in ItemManager.Instance.ItemInventory)
                {
                    if (flag) break;

                    if(item.Quantity >= 1)
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    next.OnSwitchToValidWindow();
                }
            };
            _onClick += onClick;

            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        _onClick?.Invoke();
    }

    public void OnDeselect()
    {
        foreach (var view in _selectionWindowView)
        {
            view.ChangeAlpha(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        UIManager.Instance.UpdatePrevHeader(this);

        foreach (var view in _selectionWindowView)
        {
            view.ChangeAlpha(true);
        }
    }
}
