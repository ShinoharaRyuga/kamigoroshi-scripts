using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>クエスト2の管理クラス </summary>
public class Quest02Manager : QuestManagerBase
{
    public static Quest02Manager Instance;

    [SerializeField, Tooltip("狸オブジェクト")] private GameObject _racoon = default;

    [SerializeField, Tooltip("デバッグ用。隠れたオブジェクトがわかるようにする")]
    private bool _isDebug = false;

    [Header("隠れるもの")] [SerializeField, Tooltip("つぼのリスト")]
    List<FieldObject> _vaseList = new List<FieldObject>();

    [SerializeField] private GameObject _racoonVase;
    [SerializeField] private GameObject _racoonObject;

    private List<List<FieldObject>> _hideObjList = new List<List<FieldObject>>();
    private FieldObject _hideObj = default;
    private bool _isTalk;

    /// <summary>
    /// かくれんぼ中かどうか(使わなかった場合消す)
    /// </summary>
    private bool _inHideAndSeek = false;

    public bool InHideAndSeek => _inHideAndSeek;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();

        DeadTask().Forget();
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.CurrentQuestType.Value = QuestType.Quest02;
        Debug.Log("Quest02");
    }

    public async void OnQuest02Start()
    {
        if (_inHideAndSeek)
        {
            return;
        }

        Debug.Log("かくれんぼスタート");
        SetHidePosition();
        _racoon.transform.position = new Vector3(0f, -300f, 0f);
        await UniTask.Yield(); // TalkActorListからRemoveされるのを待ってから消す
        _racoon.SetActive(false);
        _inHideAndSeek = true;
    }

    public async UniTask OnQuest02End()
    {
        if (!_inHideAndSeek)
        {
            return;
        }

        _hideObj.IsRaccoon = false;
        _inHideAndSeek = false;
        _racoonVase.SetActive(false);
        await _wallObj.WallDissolve();
        Debug.Log("かくれんぼ終了");
    }

    public async void DetectionRacoon()
    {
        if (_isTalk)
        {
            return;
        }

        _isTalk = true;
        GameObject racoon = Instantiate(_racoonObject);
        racoon.transform.position = _racoonVase.transform.position;
        racoon.transform.rotation = _racoonVase.transform.rotation;

        _racoonVase.transform.position = new Vector3(0f, -300f, 0f);

        await UniTask.Yield(); // FieldObjectListからRemoveされるのを待ってから消す
    }

    /// <summary>
    /// 狸が隠れるオブジェクトを決める。最初の狸との会話の後にこの処理を挟む。
    /// </summary>
    public void SetHidePosition()
    {
        _hideObjList.Add(_vaseList);

        var randomListNum = Random.Range(0, _hideObjList.Count);
        var list = _hideObjList[randomListNum];

        var randomObjNum = Random.Range(0, list.Count);
        _hideObj = list[randomObjNum];
        _hideObj.gameObject.SetActive(false);

        GameObject racoon = Instantiate(_racoonVase);
        racoon.transform.position = _hideObj.transform.position;
        racoon.transform.rotation = _hideObj.transform.rotation;

        _racoonVase = racoon;

        SetDebug();

        Debug.Log($"隠れたオブジェクト：{_hideObj.gameObject.name}。隠れた場所{_hideObj.gameObject.transform.position}");
    }

    private void SetDebug()
    {
        if (!_isDebug)
        {
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(_hideObj.transform.position.x,
            _hideObj.transform.position.y + 20, _hideObj.transform.position.z);
    }

    private void OnDisable()
    {
        _fieldTrigger.OnChangeScene -= TransitionField02;
    }
}