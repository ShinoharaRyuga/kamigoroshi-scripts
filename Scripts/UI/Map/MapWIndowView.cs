using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MapWIndowView : SelectionWindowView
{
    [SerializeField] MapImageView _mapView;

    protected override void OnStart()
    {
        var g = GameManager.Instance;
        var isDebug = g ? false : true;

        if (isDebug) return;

        GameManager.Instance.FieldInfo.Subscribe(x => _mapView.SetSprite(x.FieldMap)).AddTo(this);
    }
}
