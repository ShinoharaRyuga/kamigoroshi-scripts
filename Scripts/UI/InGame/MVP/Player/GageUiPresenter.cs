using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GageUiPresenter : MonoBehaviour
{
    [SerializeField] LifeSliderView _lifeSliderView;

    private void Start()
    {
        var player = PlayerManager.Instance;
        var isDebug = player ? false : true;

        if (_lifeSliderView && !isDebug)
        {
            var max = player.ParamData.MaxHP;
            _lifeSliderView.SetMaxValue(max);

            player.Hp.Subscribe(_lifeSliderView.SetValue).AddTo(this);
        }
    }
}
