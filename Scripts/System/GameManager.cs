using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using AkilliMum;
using Cysharp.Threading.Tasks.CompilerServices;

/// <summary>ゲーム全体を管理するクラス </summary>
public class GameManager : MonoBehaviour, IManager
{
    static GameManager _instance = default;
    public static GameManager Instance => _instance;

    [SerializeField, Tooltip("ステージ情報")]
    FieldInfo _stageInfo = default;

    [SerializeField]
    GameObject _player = default;

    EndingType _currentEndingType = EndingType.True;
    /// <summary>ユーザーが選択したクエスト </summary>
    ReactiveProperty<QuestType> _currentQuestType = new ReactiveProperty<QuestType>(QuestType.Quest01);
    
    int _currentFieldIndex = 0;
    ReactiveProperty<FieldModel> _fieldInfo;

    PlayerManager _playerManager = default;

    #region プロパティ
    public GameObject Player => _player;

    public PlayerManager PlayerManager => _playerManager;

    /// <summary>フィールド名のイラスト </summary>
    public IReadOnlyReactiveProperty<FieldModel> FieldInfo => _fieldInfo;
    public EndingType CurrentEndingType { get => _currentEndingType; set => _currentEndingType = value; }
    /// <summary>
    /// フィールドが切り替わった時に呼ばれるアクション
    /// </summary>
    public event Action OnEnterSceneTransitionComplete;
    /// <summary>ユーザーが選択したクエスト </summary>
    public ReactiveProperty<QuestType> CurrentQuestType { get => _currentQuestType; set => _currentQuestType = value; }
    #endregion

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = GetComponent<GameManager>();
            _playerManager = _player.GetComponent<PlayerManager>();
            DontDestroyOnLoad(gameObject);

            _fieldInfo = new ReactiveProperty<FieldModel>(_stageInfo.FieldModels[0]);
        }
    }

    public async void ChangeCurrentField(FieldType type)
    {
        var ct = this.GetCancellationTokenOnDestroy();
        await UniTask.Yield(ct);    //Startで呼び出すことを考慮して１フレーム待機させる

        _currentFieldIndex = (int)type;
        _fieldInfo.Value = _stageInfo.FieldModels[_currentFieldIndex];
        OnEnterSceneTransitionComplete?.Invoke();
        Debug.Log($"現在のフィールド : {type}");
    }

    public void DestroyObject()
    {
        _instance = null;
        Destroy(this.gameObject);
    }
}

public enum FieldType
{
    Ouka = 0,   //桜河街
    Sakuya = 1, //サクヤ神社
}

/// <summary>クエストの種類 </summary>
public enum QuestType
{
    /// <summary>敵撃破</summary>
    Quest01,
    /// <summary>かくれんぼ</summary>
    Quest02,
    /// <summary>犯人当て </summary>
    Quest03,
}