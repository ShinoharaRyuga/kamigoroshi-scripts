using TMPro;
using UnityEngine;

public class AutoAdjustTextHeight : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] float _lineSpacing = 1.0f;

    private void Awake()
    {
        // �������Ɋ�Â��ăe�L�X�g�̍�����������������
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
            // �e�L�X�g�̍����𒲐�����
            Vector2 sizeDelta = _textMeshPro.rectTransform.sizeDelta;
            sizeDelta.y = preferredHeight;
            _textMeshPro.rectTransform.sizeDelta = sizeDelta;

            // �e�L�X�g�̍s�Ԃ𒲐�����
            _textMeshPro.lineSpacing = _lineSpacing;
        }
    }
}
