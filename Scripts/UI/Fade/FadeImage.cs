using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>�t�F�[�h���s���N���X</summary>
public class FadeImage : Graphic, IFade
{
    [SerializeField] private Texture _maskTexture = null;
    [SerializeField, Range(0, 1)] private float _cutoutRange;

    public float Range
    {
        get { return _cutoutRange; }
        set
        {
            _cutoutRange = Mathf.Clamp01(value); // 0����1�͈̔͂ɐ���
            UpdateMaskCutout(_cutoutRange);
        }
    }

    protected override void Start()
    {
        base.Start();
        UpdateMaskTexture(_maskTexture);
    }

    private void UpdateMaskCutout(float range)
    {
        enabled = true;
        material.SetFloat("_Range", 1 - range);

        if (range <= 0)
        {
            this.enabled = false;
        }
    }

    public void UpdateMaskTexture(Texture texture)
    {
        material.SetTexture("_MaskTex", texture);
        material.SetColor("_Color", color);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        Range = _cutoutRange; // �v���p�e�B��ʂ��Đ����K�p
        UpdateMaskTexture(_maskTexture);
    }
#endif
}