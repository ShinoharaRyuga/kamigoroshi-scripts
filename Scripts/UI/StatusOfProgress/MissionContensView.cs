using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionContensView : MonoBehaviour
{
    [SerializeField] RectTransform _textContents;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _detailText;
    [SerializeField] TextMeshProUGUI _contentTextPrefab;
    [Space(10)]
    [SerializeField] RectTransform _rootContent;
    [SerializeField] RectTransform[] _contentsObj;
    [Space(10)]
    [SerializeField] MissionButtonView[] _buttons;

    TextMeshProUGUI[] _prevContents;

    private void Awake()
    {
        foreach (var button in _buttons)
        {
            button.SetCallback(SetText);
        }
    }

    void SetText(MissionData data)
    {
        _titleText.text = data.Title;
        AdjustTextHeight(_titleText);

        _detailText.text = data.Details;
        AdjustTextHeight(_detailText);

        //表示を切り替えるごとに新しいオブジェクトを作るか検討中
        if (_prevContents != null)
        {
            for (int i = 0; i < _prevContents.Length; i++)
            {
                var obj = _prevContents[i];

                if (!obj) continue;

                Destroy(obj.gameObject);
            }
        }

        _prevContents = new TextMeshProUGUI[data.Contents.Length];

        for(int i = 0; i < data.Contents.Length; i++)
        {
            var content = data.Contents[i];

            if (content == "") continue;

            var obj = Instantiate(_contentTextPrefab, Vector2.zero, Quaternion.identity, this._textContents.transform);
            obj.text = content;
            AdjustTextHeight(obj);
            _prevContents[i] = obj;
        }

        AdjustWindowHeight();
    }

    void AdjustWindowHeight()
    {
        if (_textContents.TryGetComponent(out VerticalLayoutGroup layout))
        {
            float height = layout.padding.top + layout.padding.bottom;
            height += _titleText.rectTransform.sizeDelta.y;
            height += _detailText.rectTransform.sizeDelta.y;
            height += layout.spacing;

            foreach (var content in _prevContents)
            {
                if (!content) continue;

                height += content.rectTransform.sizeDelta.y + layout.spacing;
            }

            Vector2 sizeDelta = _textContents.sizeDelta;
            sizeDelta.y = height;

            _textContents.sizeDelta = sizeDelta;
        }

        var rootContentsHeight = 0f;
        
        foreach(var content in _contentsObj)
        {
            if (!content) continue;

            rootContentsHeight += content.sizeDelta.y;
        }

        var size = _rootContent.sizeDelta;
        size.y = rootContentsHeight;

        _rootContent.sizeDelta = size;
    }

    void AdjustTextHeight(TextMeshProUGUI text)
    {
        if(text.TryGetComponent(out AutoAdjustTextHeight auto))
        {
            auto.AdjustTextHeight();
        }
    }
}
