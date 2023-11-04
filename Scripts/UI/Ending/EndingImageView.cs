using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingImageView : MonoBehaviour
{
    [SerializeField] Sprite _normalEnding;
    [SerializeField] Sprite _badEnding;
    [SerializeField] Sprite _trueEnding;
    [Space(10)]
    [SerializeField] Image _backGroungImage;

    public void OnSetSprite(EndingType endingType)
    {
        Sprite sprite = null;

        switch(endingType)
        {
            case EndingType.Normal:
                sprite = _normalEnding;
                break;
            case EndingType.Bad:
                sprite = _badEnding;
                break;
            case EndingType.True:
                sprite = _trueEnding;
                break;
            default:
                break;
        }

        _backGroungImage.sprite = sprite;
    }
}