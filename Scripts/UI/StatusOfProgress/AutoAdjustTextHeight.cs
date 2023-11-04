using TMPro;
using UnityEngine;

public class AutoAdjustTextHeight : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] float _lineSpacing = 1.0f;

    private void Awake()
    {
        // 文字数に基づいてテキストの高さを自動調整する
        AdjustTextHeight();
    }

    private void Reset()
    {
        TryGetComponent(out _textMeshPro);
    }

    [ContextMenu("Init")]
    public void AdjustTextHeight()
    {
        float preferredHeight = _textMeshPro.preferredHeight;
        float currentHeight = _textMeshPro.rectTransform.sizeDelta.y;

        if (Mathf.Abs(preferredHeight - currentHeight) > 0.01f)
        {
            // テキストの高さを調整する
            Vector2 sizeDelta = _textMeshPro.rectTransform.sizeDelta;
            sizeDelta.y = preferredHeight;
            _textMeshPro.rectTransform.sizeDelta = sizeDelta;

            // テキストの行間を調整する
            _textMeshPro.lineSpacing = _lineSpacing;
        }
    }
}
