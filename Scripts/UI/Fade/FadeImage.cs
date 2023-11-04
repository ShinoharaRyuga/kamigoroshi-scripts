using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>フェードを行うクラス</summary>
public class FadeImage : Graphic, IFade
{
    [SerializeField] private Texture _maskTexture = null;
    [SerializeField, Range(0, 1)] private float _cutoutRange;

    public float Range
    {
        get { return _cutoutRange; }
        set
        {
            _cutoutRange = Mathf.Clamp01(value); // 0から1の範囲に制限
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
        Range = _cutoutRange; // プロパティを通じて制約を適用
        UpdateMaskTexture(_maskTexture);
    }
#endif
}