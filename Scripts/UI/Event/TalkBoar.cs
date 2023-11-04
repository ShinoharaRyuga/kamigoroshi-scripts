using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TalkBoar : TalkActor
{
    [SerializeField] private Quest03Manager _quest03Manager;
    [SerializeField] private int _number;
    [SerializeField] private EventData[] _nextEventData1;
    [SerializeField] private EventData[] _nextEventData2;
    
    public override void Talk()
    {
        if (!_eventData) return;

        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Select", 1f);

        OnCallback();
        if (_quest03Manager.IsTalkers.Count(IsTalkers => IsTalkers) == 1)　//最初に会話したら
        {
            Debug.Log("最初");
            EventManager.Instance.EventStart(_eventData, null, () =>
            {
                SetPlayerAndYajimaTalkPos();
            }, _nextEventData1);
        }
        else if (_quest03Manager.IsTalkers.All(IsTalkers => IsTalkers))　//すべて会話したら
        {
            Debug.Log("最後");
            EventManager.Instance.EventStart(_eventData, async () =>
            {
                await Quest03Manager.Instance.ExchangeMemo();
            }, () =>
            {
                SetPlayerAndYajimaTalkPos();
            }, _nextEventData2);
        }
        else //それ以外の会話
        {
            Debug.Log("その他");
            EventManager.Instance.EventStart(_eventData, null, () =>
            {
                SetPlayerAndYajimaTalkPos();
            });
        }

    }

    /// <summary>
    /// Talk時のPlayerとYajimaの位置をセットする
    /// </summary>
    private void SetPlayerAndYajimaTalkPos()
    {
        if (_player && _yajima)
        {
            //座標用の引数と回転用の引数を用意
            _player.transform.position = _playerMoveOffset.position;
            _yajima.transform.position = _yajimaMoveOffset.position;

            _player.transform.LookAt(transform);
            _yajima.transform.LookAt(transform);
        }
    }

    protected override void OnCallback()
    {
        Debug.Log("ボア!");
        _quest03Manager.OnTalk(_number);
    }
}