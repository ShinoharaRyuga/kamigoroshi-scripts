using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;
using Cinemachine;

public class FieldObject : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private FieldTargetNameView _objNamePlate;
    [SerializeField] private CinemachineVirtualCamera _focusCamera;
    [Header("通常のオブジェクトテキストデータ")]
    [SerializeField] private EventData _normalObjectEventData;
    [Header("狸だった場合のオブジェクトデータ")]
    [SerializeField] private EventData _tanukiTextData;

    [Header("ゲットアイテムオブジェクトデータ")]
    [SerializeField] private bool _isGetItem;
    [SerializeField] private EventData[] _getItemData;
    [SerializeField] private ItemType _itemType;

    private bool _isAlreadyGetItem;
    private bool _isRaccoon = false;
    public bool IsRaccoon
    {
        set { this._isRaccoon = value; }
        get { return _isRaccoon; }
    }

    private void Start()
    {
        _objNamePlate.Initialize(_sprite);
        _objNamePlate.gameObject.SetActive(true);
    }

    public void Enter()
    {
        _objNamePlate.OnShow();
    }

    public void Exit()
    {
        _objNamePlate.OnHide();
    }

    public void Talk()
    {
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Select", 1f);

        if (!_isRaccoon)
        {
            if (_isGetItem && !_isAlreadyGetItem)
            {
                CameraManager.Instance.SetTargetCamera(_focusCamera);

                _isAlreadyGetItem = true;
                EventManager.Instance.EventStart(_normalObjectEventData, () =>
                {
                    if (_itemType != ItemType.None)
                    {
                        ItemManager.Instance.AddItem(_itemType);
                    }
                }, () =>
                {
                    //Playerの座標と回転を変える関数に変更予定（PlyaerManager or GameManager）
                    var p = GameObject.FindGameObjectWithTag("Player");

                    if (p)
                    {
                        //回転用の引数を用意
                        p.transform.LookAt(transform);
                    }
                }, _getItemData);
            }
            else
            {
                CameraManager.Instance.SetTargetCamera(_focusCamera);
                EventManager.Instance.EventStart(_normalObjectEventData, null, null);
            }
        }
        else
        {
            //たぬきだった時の処理
            Debug.Log("これは狸です");
            EventManager.Instance.EventStart(_tanukiTextData, null, () =>
            {
                //Playerの座標と回転を変える関数に変更予定（PlyaerManager or GameManager）
                var p = GameObject.FindGameObjectWithTag("Player");

                if (p)
                {
                    //回転用の引数を用意
                    p.transform.LookAt(transform);
                }
            });
        }
    }
}