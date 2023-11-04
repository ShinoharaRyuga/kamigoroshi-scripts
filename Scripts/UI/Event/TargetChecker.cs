using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// アイテムを入手するクラス　プレイヤーに子要素としてアタッチする想定
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract partial class TargetChecker : MonoBehaviour
{
    [Tooltip("入手範囲のコライダー")]
    private SphereCollider _collider;

    protected ReactiveProperty<bool> _isEnable = new ReactiveProperty<bool>();

    /// <summary>
    /// ヘルプテキストを表示する為のReactiveProperty
    /// </summary>
    public IReadOnlyReactiveProperty<bool> IsEnable => _isEnable;

    /// <summary>
    /// 範囲内にあるアイテムの関数を呼ぶ
    /// </summary>
    public abstract void OnFuncCall();
    public abstract bool OnCheck();
}

#if UNITY_EDITOR
//　インスペクターでコライダーの設定をできるようにするための処理
//　Editor上でだけ動けばいいので、partialにして条件付きコンパイルにしている
public partial class TargetChecker : MonoBehaviour
{
    [Header("Editor")]
    [SerializeField, Tooltip("入手可能半径")]
    private float _colliderRadius = 3f;

    //Inspector上で値が変更されたときに呼ばれるUnityコールバック
    private void OnValidate()
    {
        if (_collider == null)
        {
            //初回のみGetComponentする
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }

        _collider.radius = _colliderRadius;
    }
}
#endif
