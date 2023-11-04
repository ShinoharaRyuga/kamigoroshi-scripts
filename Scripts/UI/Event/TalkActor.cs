using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class TalkActor : MonoBehaviour
{
    [SerializeField] Sprite _sprite;
    [SerializeField] FieldTargetNameView _npcNamePlate;
    [SerializeField] protected EventData _eventData;
    [Space(10)] 
    [SerializeField] protected Transform _playerMoveOffset;
    [SerializeField] protected Transform _yajimaMoveOffset;
    protected GameObject _player;
    protected GameObject _yajima;

    private void Start()
    {
        _npcNamePlate.Initialize(_sprite);
        _npcNamePlate.gameObject.SetActive(true);

        _player = GameObject.FindGameObjectWithTag("Player");
        _yajima = GameObject.FindGameObjectWithTag("Yajima");
    }

    protected virtual void OnCallback()
    {
    }

    public void Enter()
    {
        _npcNamePlate.OnShow();
    }

    public void Exit()
    {
        _npcNamePlate.OnHide();
    }

    public virtual void Talk()
    {
        if (!_eventData) return;

        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Select", 1f);

        EventManager.Instance.EventStart(_eventData, () =>
        {
            OnCallback();
        }, () =>
        {
            //Playerの座標と回転を変える関数に変更予定（PlyaerManager or GameManager）
            _player = GameObject.FindGameObjectWithTag("Player");
            _yajima = GameObject.FindGameObjectWithTag("Yajima");

            if (_player && _yajima)
            {
                //座標用の引数と回転用の引数を用意
                _player.transform.position = _playerMoveOffset.position;
                _yajima.transform.position = _yajimaMoveOffset.position;

                _player.transform.LookAt(transform);
                _yajima.transform.LookAt(transform);
            }
        });
    }
}