using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;

public class ItemButtonView : MonoBehaviour, ISelectHandler
{
    [SerializeField] Image _iconImage;
    [SerializeField] TextMeshProUGUI _quantityText;
    [Space(10)]
    [SerializeField] Sprite _existsDisabledSprite;
    [SerializeField] Sprite _doesNotExistDisabledSprite;

    ItemBase _item;
    Button _button;

    public event Action<ItemBase> OnSelectEvent;

    private void Awake()
    {
        TryGetComponent(out _button);

        ResetView();
    }

    public void Setup(Action<Selectable> selectionEvent, SelectionWindowView nextWindow)
    {
        if (TryGetComponent(out Button button))
        {
            button.onClick.RemoveAllListeners();

            if (_item.ItemData.IsKeyItem) return;

            button.onClick.AddListener(nextWindow.OnSwitchToValidWindow);
            button.onClick.AddListener(() => selectionEvent(button));
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelectEvent?.Invoke(_item);
    }

    /// <summary>
    /// View‚Ì‰æ‘œ‚ÆŒÂ”‚Ì•\¦‚ğ•ÏX
    /// </summary>
    /// <param name="item"></param>
    public void OnUpdateButton(ItemBase item)
    {
        if(!item || item.Quantity <= 0)
        {
            ResetView();
            return;
        }

        _item = item;

        _button.interactable = true;
        ChangeSpriteState(_existsDisabledSprite);

        _iconImage.enabled = true;
        _iconImage.sprite = item.ItemData.ItemSprite;

        var str = new StringBuilder("~");
        str.Append(item.Quantity);
        _quantityText.text = str.ToString();
    }

    void ResetView()
    {
        _item = null;
        _iconImage.enabled = false;
        _quantityText.text = string.Empty;
        _button.interactable = false;
        ChangeSpriteState(_doesNotExistDisabledSprite);
    }

    void ChangeSpriteState(Sprite sprite)
    {
        var state = _button.spriteState;
        state.disabledSprite = sprite;

        _button.spriteState = state;
    }
}
