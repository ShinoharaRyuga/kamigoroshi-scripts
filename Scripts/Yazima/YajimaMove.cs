using UnityEngine;
using UnityEngine.AI;

public class YajimaMove : MonoBehaviour, IPause, IManager
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private PlayerParameter _parameter;
    [SerializeField] private float _distance = 0.5f;
    [SerializeField] private float _offest = 3f;

    private float _speed;
    private float _decelerationDistance;
    private NavMeshAgent _nav;
    private Animator _animator = default;
    private bool _isDecelerating = false;

    public static YajimaMove Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _speed = _parameter.MoveSpeed;
        _decelerationDistance = _distance;
        _nav.speed = _speed;
    }

    private void Update()
    {
        if (_nav.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return;
        }

        if (_target != null)
        {
            Vector3 pos = _target.position;
            float radius = _distance + _target.localScale.magnitude / _offest;
            float center = Vector3.Distance(transform.position, pos);

            if (!_nav.isStopped)
            {
                Vector3 direction = (_target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }

            if (center <= radius)
            {
                _nav.isStopped = true;
                _nav.velocity = Vector3.zero;
                _isDecelerating = false;
            }
            else
            {
                _nav.isStopped = false;
                if (center <= _decelerationDistance && !_isDecelerating)
                {
                    float factor = center / _decelerationDistance;
                    _nav.velocity = _nav.velocity.normalized * _speed * factor;
                    _isDecelerating = true;
                }
                else if (center > _decelerationDistance)
                {
                    _isDecelerating = false;
                }

                _nav.SetDestination(pos);
            }

            _animator.SetFloat("Speed", _nav.velocity.magnitude);
        }
    }

    public void Pause()
    {
        _nav.isStopped = true;
        _isDecelerating = false;
    }

    public void Resume()
    {
        _nav.isStopped = false;
        _nav.SetDestination(_target.position);
        _isDecelerating = false;
    }

    /// <summary>
    /// NavMeshAgentのenabledの切り替え
    /// </summary>
    /// <param name="active"></param>
    public void SetNavMeshAgent(bool isActive)
    {
        _nav.enabled = isActive;
    }

    /// <summary>
    /// 八島を指定の場所にワープさせる
    /// </summary>
    /// <param name="vec"></param>
    public void Warp(Vector3 vec)
    {
        _nav.Warp(vec);
    }

    public void DestroyObject()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
