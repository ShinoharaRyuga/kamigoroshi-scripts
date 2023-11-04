using UnityEngine;
using InputControl;
using PlayerState;
using UniRx;
using Cinemachine;

/// <summary>プレイヤーが持つ機能を管理するクラス </summary>
[RequireComponent(typeof(Rigidbody), typeof(PlayerStateMachineController), typeof(PlayerEffectController))]
[RequireComponent(typeof(PlayerParamController), typeof(PlayerAnimationController))]
public class PlayerManager : MonoBehaviour, IDamage, IPause, IItemChangeParam, IManager
{
    [SerializeField, Header("プレイヤーカメラ")]
    Camera _playerCamera = default;
    [SerializeField, Header("攻撃範囲の中心")]
    Vector3 _attackRangeCenter = Vector3.zero;
    [SerializeField, Header("攻撃範囲(Box用)")]
    Vector3 _attackVec = default;
    [SerializeField, Header("アイテム取得クラス")]
    GetItem _getItem = default;
    [SerializeField, Header("会話用のクラス")]
    ActorTalker _actorTalker = default;
    [SerializeField, Header("調べる用のクラス")]
    GetObject _getObject = default;
    [SerializeField, Header("TargetCheckerControllerのクラス")]
    TargetCheckerController _targetCheckerController = default;
    [SerializeField, Header("刀")]
    GameObject _sword = default;
    [SerializeField, Tooltip("Playerの回転補正値")]
    float _rotationValue = 100f;
    [SerializeField, Tooltip("ゲームオーバークラス")]
    ExecuteGameOverProcess _executeGameOverProcess = default;
    /// <summary>ポーズ中 </summary>
    bool _isPause = false;
    /// <summary>無敵かどうか</summary>
    bool _isInvincible = false;

    Rigidbody _rb = default;
    CinemachineBrain _cameraBrain = default;
    PlayerStateMachineController _stateMachineController;
    PlayerAnimationController _animationController;
    PlayerParamController _paramController;
    PlayerEffectController _effectController;

    #region プロパティ
    public bool IsPause => _isPause;
    public bool IsInvicible 
    {
        get { return _isInvincible; }
        set { this._isInvincible = value; }
    } 
    public float MoveSpeed => _paramController.MoveSpeed;
    public float MaxHp => _paramController.ParamData.MaxHP;
    public float _vertical => PlayerInputs.Instance.InputDirection.y;
    public float _horizontal => PlayerInputs.Instance.InputDirection.x;
    public PlayerStateMachineController StateMachineController => _stateMachineController;
    public PlayerAnimationController AnimationController => _animationController;
    public PlayerEffectController EffectController => _effectController;
    public GetItem PlayerGetItem => _getItem;
    public ActorTalker PlayerActorTalker => _actorTalker;
    public GetObject PlayerGetObject => _getObject;
    public Camera PlayerCamera => _playerCamera;
    public float RotationValue => _rotationValue;
    public Rigidbody Rb => _rb;
    /// <summary>体力 </summary>
    public ReactiveProperty<float> Hp => _paramController.HP;
    /// <summary>攻撃力 </summary>
    public ReactiveProperty<float> AttackPower => _paramController.AttackPower;
    /// <summary>パラメーターデータ 基本触る必要はないです</summary>
    public PlayerParameter ParamData => _paramController.ParamData;
    public ExecuteGameOverProcess ExecuteGameOverProcess => _executeGameOverProcess;
    #endregion

    public static PlayerManager Instance { get; private set; }
    private void Awake()
    {
        CheckInstance();
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            if (GameManager.Instance != null)
            {
                PauseManager.Instance.AddPauseObject(this);
            }

            Initialize();
            GameManager.Instance.CurrentQuestType.Subscribe((questType) =>
            {
                Debug.Log(questType);
                _animationController.SetAnimatorController(questType, _sword);
            }).AddTo(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetAttackRangeCenterOfSword(), _attackVec);
    }

    // TODO：現在はBoxで当たり判定をとっているかShereで当たり判定をとるかどうかはデバッグしながら感触を確かめたい
    /// <summary>攻撃を行う </summary>
    public void Attack()
    {
        Collider[] cols = Physics.OverlapBox(GetAttackRangeCenterOfSword(), _attackVec, _sword.transform.rotation);

        foreach (var col in cols)
        {
            if (col.gameObject != this.gameObject && col.gameObject != _sword.gameObject && col.gameObject.TryGetComponent(out IDamage enemy))
            {
                enemy.AddDamage(AttackPower.Value);
                var effectPos = col.ClosestPoint(_sword.transform.position);
                _effectController.PlayHitEffect(effectPos);
            }
        }
        
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Swing", 0.5f);
    }

    public void AddDamage(float damageValue)
    {
        _paramController.GetDamage(damageValue, _isInvincible);
        _stateMachineController.StateMachine.ChangeState(PlayerStateType.Damage);
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_Player_Damage");
    }

    public void Pause()
    {
        _isPause = true;
        _stateMachineController.StateMachine.ChangeState(PlayerStateType.Idle);
        AudioManager.Instance.StopSound(SoundType.SE, "SE_Run_Ground");

        if (!CameraManager.Instance.IsCameraMoved) _cameraBrain.enabled = false;
    }

    public void Resume()
    {
        _isPause = false;

        if (!CameraManager.Instance.IsCameraMoved) _cameraBrain.enabled = true;
    }

    /// <summary>即時発動のアイテムの処理を行う </summary>
    /// <param name="targetParam">対象のパラメーター</param>
    /// <param name="value">増減値</param>
    public void ItemParamChangeActive(ParameterType targetParam, float value)
    {
        _paramController.ItamEffectActive(targetParam, value);
    }

    /// <summary>持続アイテムの処理を行う </summary>
    /// <param name="targetParam">対象のパラメーター</param>
    /// <param name="duration">持続時間</param>
    /// <param name="addValue">増減値</param>
    public void ItemParamChangeHold(ParameterType targetParam, float duration, float addValue)
    {
        _paramController.ItemEffectHold(targetParam, duration, addValue);
    }

    /// <summary>初期化処理 </summary>
    private void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        _stateMachineController = GetComponent<PlayerStateMachineController>();
        _animationController = GetComponent<PlayerAnimationController>();
        _paramController = GetComponent<PlayerParamController>();
        _effectController = GetComponent<PlayerEffectController>();
        _cameraBrain = _playerCamera.gameObject.GetComponent<CinemachineBrain>();

        _paramController.InitializeParameter(Dead);
    }

    /// <summary>死亡処理 </summary>
    private void Dead()
    {
        _isPause = true;
        _cameraBrain.enabled = false;
    }

    // 刀を中心として当たり判定を設定
    /// <summary> 攻撃範囲の中心を計算して取得する </summary>
    /// <returns>刀の攻撃範囲の中心座標</returns>
    private Vector3 GetAttackRangeCenterOfSword()
    {
        Vector3 center = _sword.transform.position + _sword.transform.forward * _attackRangeCenter.z
           + _sword.transform.up * _attackRangeCenter.y
           + _sword.transform.right * _attackRangeCenter.x;
        return center;
    }

    public void DestroyObject()
    {
        PauseManager.Instance.RemovePauseObject(this);
        _targetCheckerController.OnRemoveInput();
        Instance = null;
        Destroy(this.gameObject);
    }
}
