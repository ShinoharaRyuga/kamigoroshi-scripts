using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapImageView : MonoBehaviour
{
    Image _mapImage;

    private void Awake()
    {
        TryGetComponent(out _mapImage);
    }

    public void SetSprite(Sprite sprite)
    {
        _mapImage.sprite = sprite;
    }
}
