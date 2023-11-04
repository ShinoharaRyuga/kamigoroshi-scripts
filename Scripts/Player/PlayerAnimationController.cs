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
    [Header("���������Ă���ꍇ��AnimatorController(�N�G�X�g1)")]
    [SerializeField]
    RuntimeAnimatorController _carryingKatanaController = default;

    [Header("���������Ă��Ȃ��ꍇ��AnimatorController(�N�G�X�g2��3)")]
    [SerializeField]
    RuntimeAnimatorController _notingKatanaController = default;

    int _currentComboCount = 0;

    /// <summary>�A�j���[�V�������Đ������ǂ���</summary>
    bool _isPlayAnim = false;

    bool _isCarryingKatana;

    PlayerManager _playerManager;

    Animator _animator = default;
    public Animator PlayerAnimator => _animator;

    /// <summary>�A�j���[�V�������Đ������ǂ���</summary>
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

    /// <summary>�eAnimationState��Subscribe����</summary>
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

        var exitState = trigger.OnStateExitAsObservable()   //LightAttack�̃X�e�[�g���Đ��I�����̏���
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

        exitState = trigger.OnStateExitAsObservable()   //Damage�̃X�e�[�g���Đ��I�����̏���
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

            if (stateInfo.IsName("Dead.KnockDown"))     //���S������
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

    /// <summary>LightAttack�̃A�j���[�V�������Đ����� </summary>
    public void StartLightAttackAnim()
    {
        if(!_isCarryingKatana)
        {
            return;
        }
        _isPlayAnim = true;
        _animator.SetTrigger("LightAttack");
    }

    /// <summary>�_���[�W�A�j���[�V�������Đ����� </summary>
    public void StartDamageAnim()
    {
        _isPlayAnim = true;
        _animator.SetTrigger("Damage");
    }

    /// <summary>�ړ��A�j���[�V�������Đ� </summary>
    /// <param name="moveSpeed">�ړ����x</param>
    public void MoveAnim(float moveSpeed)
    {
        _animator.SetFloat("Move", moveSpeed);
    }

    public void StartDeadAnim()
    {
        _isPlayAnim = true;
        _animator.SetTrigger("Dead");
    }

    //TODO:����GetItem��Animation���ǉ����ꂽ�炱����Ă�
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

    /// <summary>�Đ����̃X�e�[�g�������Ɏw�肳��Ă�����̂��ǂ������ׂ� </summary>
    bool CheckPlayingState(string stateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
