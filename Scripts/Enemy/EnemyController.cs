using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IPause, IDamage
{
    [Header("ステータス")] [SerializeField] private float _maxHp = 10;
    [SerializeField] private float _currentHp = 10;
    [SerializeField] private float _attack = 10;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _trackingRange = 3f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _interval = 1.5f;

    [Header("UI")] [SerializeField] private Slider _hpSlider;

    [Header("エフェクト")] [SerializeField] private ParticleSystem _deathEffect;

    [NonSerialized] public bool _isSearching = false;

    private float _saveSpeed;
    private AudioManager _audioManager;
    private PauseManager _pauseManager;
    private NavMeshAgent _navMeshAgent;

    public float MaxHP => _maxHp;
    public ReactiveProperty<float> CurrentHP => new ReactiveProperty<float>(_currentHp);
    public ReactiveProperty<float> Attack => new ReactiveProperty<float>(_attack);
    public float Speed => _speed;
    public float TrackingRange => _trackingRange;
    public float AttackRange => _attackRange;
    public float Interval => _interval;

    private Subject<Unit> _deactivationSubject = new Subject<Unit>();
    public IObservable<Unit> OnDeactivation => _deactivationSubject;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _pauseManager = PauseManager.Instance;

        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _maxHp;

        _pauseManager.AddPauseObject(this);

        this.ObserveEveryValueChanged(obj => obj.gameObject.activeSelf)
            .Where(isActive => !isActive)
            .Subscribe(_ => _deactivationSubject.OnNext(Unit.Default))
            .AddTo(this);
    }


    public void Pause()
    {
        _saveSpeed = _speed;
        _navMeshAgent.speed = 0;
    }

    public void Resume()
    {
        _navMeshAgent.speed = _saveSpeed;
    }

    public void OnFight()
    {
        Quest01Manager.Instance.OnPlaySound();
    }

    public void OnUntraceable()
    {
        Quest01Manager.Instance.OnStopSound().Forget();
    }

    public void AddDamage(float damageValue)
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE,"Voice_Enemy_Damage");

        if ((CurrentHP.Value -= damageValue) <= 0)
        {
            //死亡処理
            Debug.Log("死んだ");
            _pauseManager.RemovePauseObject(this);
            Quest01Manager.Instance.OnStopSound().Forget();
            //AudioManager.Instance.PlaySound(SoundType.VOICE,"Voice_Enemy_Damage");
            ParticleSystem particle = Instantiate(_deathEffect,
                new Vector3(transform.position.x, _deathEffect.transform.position.y, transform.position.z),
                Quaternion.identity);
            particle.Play();
            this.gameObject.SetActive(false);
        }
        else
        {
            _hpSlider.value = _currentHp;
        }
    }

    public void OnAttack()
    {
        AudioManager.Instance.PlaySound(SoundType.VOICE,"Voice_Enemy_Attack");

        Vector3 center = transform.position + transform.forward * _trackingRange / 4f +
                         Vector3.up * (transform.localScale.y);
        Vector3 extents = new Vector3(_trackingRange / 3f, transform.localScale.y, _trackingRange);
        Collider[] colliders = Physics.OverlapBox(center, extents, transform.rotation);

        foreach (Collider hitCollider in colliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.gameObject.CompareTag("Player"))
            {
                IDamage damage = hitCollider.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.AddDamage(_attack);
                }
            }
        }
    }

    private void OnDestroy()
    {
        _pauseManager.RemovePauseObject(this);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position + transform.forward * _trackingRange / 4f +
                         Vector3.up * (transform.localScale.y);
        Vector3 extents = new Vector3(_trackingRange / 3f, transform.localScale.y, _trackingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, extents);
    }
}