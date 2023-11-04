using Cysharp.Threading.Tasks;
using InputControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


/// <summary>
/// アイテムを入手するクラス　プレイヤーに子要素としてアタッチする想定
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public partial class ActorTalker : TargetChecker
{
    Transform _thisTransform;
    List<TalkActor> _talkActors = new List<TalkActor>();

    private void Awake()
    {
        TryGetComponent(out _thisTransform);
    }

    //アイテムが入手可能範囲に入ったことを検知する
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out TalkActor actor)) return;

        _talkActors.Add(actor);

        if (!EventManager.Instance.IsPlaying)
        {
            actor.Enter();
        }

        if (_talkActors.Count > 0)
        {
            _isEnable.Value = true;
        }
    }

    //アイテムが入手可能範囲から出たことを検知する
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out TalkActor actor)) return;

        _talkActors.Remove(actor);
        actor.Exit();

        if (_talkActors.Count <= 0)
        {
            _isEnable.Value = false;
        }
    }

    public override bool OnCheck()
    {
        if (_talkActors.Count <= 0) return false;

        return true;
    }

    /// <summary>
    /// 範囲内にあるアイテムの関数を呼ぶ
    /// </summary>
    public override void OnFuncCall()
    {
        if (_talkActors.Count <= 0) return;

        TalkActor target = default;
        var minDis = 100f;    //取り合えず大きめな値を入れておく

        //複数のNPCが範囲内にいた場合、近いほうを対象にする
        foreach(var actor in _talkActors)
        {
            var dis = Vector2.Distance(_thisTransform.position, actor.transform.position);

            if (dis >= minDis) continue;

            minDis = dis;
            target = actor;
        }

        target.Talk();
    }
}