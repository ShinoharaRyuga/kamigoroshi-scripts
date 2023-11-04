using System;
using System.Linq;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>�N�G�X�g3�̊Ǘ��N���X </summary>
public class Quest03Manager : QuestManagerBase
{
    public static Quest03Manager Instance;

    [SerializeField] private GameObject[] _boars = new GameObject[3];

    [SerializeField, Tooltip("イベント後に取得するメモ帳のアイテムデータ")] private ItemData _normalMemoData;
    [SerializeField, Tooltip("イベント後に取得するメモ帳のアイテムデータ")] private ItemData _afterEventMemoData;

    private ItemBase _itemBase;

    private bool[] _isTalkers = new bool[3] { false, false, false };

    public bool[] IsTalkers => _isTalkers;

    [SerializeField] bool _isDebug = default;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        Initialize();
        if (_isDebug)
        {
            for (int i = 0; i < _isTalkers.Length; i++)
            {
                _isTalkers[i] = true;
            }
        }

        DeadTask().Forget();
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.CurrentQuestType.Value = QuestType.Quest03;
        Debug.Log("Quest03");
    }

    public void OnTalk(int num)
    {
        _isTalkers[num - 1] = true;
    }

    public void BoarDestroy()
    {
        Array.ForEach(_boars, g => g.transform.position = new Vector3(0, -300f, 0));
    }

    public async UniTask WallDissolve()
    {
        await _wallObj.WallDissolve();
    }
    public async UniTask ExchangeMemo()
    {
        ItemManager.Instance.DisposeItem(_normalMemoData.ItemType);
        _itemBase = ItemManager.Instance.AddItem(_afterEventMemoData.ItemType);
        await UniTask.CompletedTask;
    }
}