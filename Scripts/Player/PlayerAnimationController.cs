using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using System;
using InputControl;
using UniRx.InternalUtil;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("刀を持っている場合のAnimatorController(クエスト1)")]
    [SerializeField]
    RuntimeAnimatorController _carryingKatanaController = default;

    [Header("刀を持っていない場合のAnimatorController(クエスト2と3)")]
    [SerializeField]
    RuntimeAnimatorController _notingKatanaController = default;

    int _currentComboCount = 0;

    /// <summary>アニメーションが再生中かどうか</summary>
    bool _isPlayAnim = false;

    bool _isCarryingKatana;

    PlayerManager _playerManager;

    Animator _animator = default;
    public Animator PlayerAnimator => _animator;

    /// <summary>アニメーションが再生中かどうか</summary>
    public bool IsPlayAnim { get => _isPlayAnim; }

    public bool IsCarryingKatana { get => _isCarryingKatana; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        Initialize();
    }

    /// <summary>各AnimationStateのSubscribe処理</summary>
    private void Initialize()
    {
        var trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();

        var enterState = trigger.OnStateEnterAsObservable()
            .Subscribe(onStateInfo =>
            {
                var stateInfo = onStateInfo.StateInfo;

                if (stateInfo.IsName("LightAttack.LightAttack01") || stateInfo.IsName("LightAttack.LightAttack02") || stateInfo.IsName("LightAttack.LightAttack03"))
                {
                    _currentComboCount++;
                }

            }).AddTo(this);

        var exitState = trigger.OnStateExitAsObservable()   //LightAttackのステートが再生終了時の処理
            .Subscribe(onStateInfo =>
            {
                var stateInfo = onStateInfo.StateInfo;

                if (stateInfo.IsName("LightAttack.LightAttack01") || stateInfo.IsName("LightAttack.LightAttack02") || stateInfo.IsName("LightAttack.LightAttack03"))
                {
                    _currentComboCount--;
                    if (_currentComboCount <= 0)
                    {
                        _currentComboCount = 0;
                        _isPlayAnim = false;
                        _animator.ResetTrigger("LightAttack");
                    }
                }
            }).AddTo(this);

        exitState = trigger.OnStateExitAsObservable()   //Damageのステートが再生終了時の処理
          .Subscribe(onStateInfo =>
          {
              var stateInfo = onStateInfo.StateInfo;

              if (stateInfo.IsName("Damage"))
              {
                  _isPlayAnim = false;

                  if (_playerManager.Hp.Value <= 0)
                  {
                      _playerManager.StateMachineController.StateMachine.ChangeState(PlayerState.PlayerStateType.Dead);
                  }
                  else
                  {
                      _playerManager.StateMachineController.StateMachine.ChangeState(PlayerState.PlayerStateType.Idle);
                  }
              }
          }).AddTo(this);

        exitState = trigger.OnStateExitAsObservable()
        .Subscribe(onStateInfo =>
        {
            var stateInfo = onStateInfo.StateInfo;

            if (stateInfo.IsName("Dead.KnockDown"))     //死亡時処理
            {
                //GameManager.Instance.FadeOut();
            }
        }).AddTo(this);

        exitState = trigger.OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                var stateInfo = onStateInfo.StateInfo;

                if (stateInfo.IsName("GetItem"))
                {
                    _isPlayAnim = false;
                }
            }).AddTo(this);

        exitState = trigger.OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                var stateInfo = onStateInfo.StateInfo;

                if (stateInfo.IsName("DodgeLeft") || stateInfo.IsName("DodgeRight"))
                {
                    _isPlayAnim = false;
                }
            }).AddTo(this);
    }

    /// <summary>LightAttackのアニメーションを再生する </summary>
    public void StartLightAttackAnim()
    {
        if(!_isCarryingKatana)
        {
            return;
        }
        _isPlayAnim = true;
        _animator.SetTrigger("LightAttack");
    }

    /// <summary>ダメージアニメーションを再生する </summary>
    public void StartDamageAnim()
    {
        _isPlayAnim = true;
        _animator.SetTrigger("Damage");
    }

    /// <summary>移動アニメーションを再生 </summary>
    /// <param name="moveSpeed">移動速度</param>
    public void MoveAnim(float moveSpeed)
    {
        _animator.SetFloat("Move", moveSpeed);
    }

    public void StartDeadAnim()
    {
        _isPlayAnim = true;
        _animator.SetTrigger("Dead");
    }

    //TODO:今後GetItemのAnimationが追加されたらこれを呼ぶ
    public void StartGetItemAnim()
    {
        _isPlayAnim = true;
        _animator.SetTrigger("GetItem");
    }

    public void StartDodgeActAnim(float moveDirX)
    {
        _isPlayAnim = true;
        if(moveDirX < 0)
        {
            _animator.SetTrigger("DodgeLeft");
        }
        else if(moveDirX >= 0)
        {
            _animator.SetTrigger("DodgeRight");
        }
    }

    public void SetAnimatorController(QuestType type, GameObject obj)
    {
        if (type == QuestType.Quest01)
        {
            _isCarryingKatana = true;
            _animator.runtimeAnimatorController = _carryingKatanaController;
            obj.SetActive(true);
        }
        else
        {
            _isCarryingKatana = false;
            _animator.runtimeAnimatorController = _notingKatanaController;
            obj.SetActive(false);
        }
    }

    /// <summary>再生中のステートが引数に指定されているものかどうか調べる </summary>
    bool CheckPlayingState(string stateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
