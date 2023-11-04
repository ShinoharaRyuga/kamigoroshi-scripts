using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using CriWare;
using UnityEngine;
using Cinemachine;

/// <summary>フィールド02（神社）の管理クラス </summary>
public class Field02Manager : MonoBehaviour
{
    public static Field02Manager Instance;

    [SerializeField, Tooltip("開始時のプレイヤー位置")]
    Transform _firstPlayerPosition = default;

    [SerializeField, Tooltip("開始時のプレイヤー位置")]
    Transform _firstYajimaPosition = default;

    [SerializeField]
    CinemachineVirtualCamera _eventCamera = default;

    [SerializeField] FadeController _fadeController = default;

    /// <summary>
    /// クエスト3からField02に遷移した時に発生する会話データ
    /// </summary>
    [SerializeField] private EventData _quest03TalkData = default;

    /// <summary>
    /// クエスト1、2からField02に遷移した時に発生する会話データ
    /// </summary>
    [SerializeField] private EventData _talkData = default;

    [SerializeField] private EventData _normalEndTalkData = default;

    [SerializeField] private GameObject[] _boarActors = new GameObject[3];

    [SerializeField, Tooltip("結界の要アイテム")] private GameObject _keyItem = default;

    [SerializeField] private CriAtomSource _criAtomSource;
    
    [SerializeField] private string[] _bgmCueName;
    [SerializeField] private string[] _seCueName;

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
    }

    /// <summary>初期化処理 </summary>
    private void Initialize()
    {
        PlayerManager.Instance.transform.position = _firstPlayerPosition.position;
        YajimaMove.Instance.transform.position = _firstYajimaPosition.position;
        YajimaMove.Instance.Warp(_firstYajimaPosition.position);

        CheckQuestType();
    }

    public Coroutine EventGameClear()
    {
        Debug.Log("クリア");
        AudioManager.Instance.PlaySound(SoundType.VOICE, "Voice_NPC1_2", 1f);
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Eat", 1f);
        GameManager.Instance.CurrentEndingType = EndingType.True;
        Array.ForEach(_boarActors, g => Destroy(g));
        _keyItem.SetActive(true);
        return StartCoroutine(WaitTime());
    }

    public async UniTask GameEnd(string scene)
    {
        var async = EventManager.Instance.SceneChanger.ReturnAsyncOperation(scene);
        async.allowSceneActivation = false;

        await EventManager.Instance.FadeController.FadeOut();
        Debug.Log("Tipsを表示します");

        await StartCoroutine(WaitTime());
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Mirror_Break", 1f);
        _criAtomSource.Stop();
        StopAudio();
        await StartCoroutine(WaitTime());

        await EventManager.Instance.LoadingTips.OnOpen();

        async.allowSceneActivation = true;

        Debug.Log($"{scene}に遷移します");
    }

    public void EventGameOver()
    {
        Debug.Log("ゲームオーバー");
        _criAtomSource.Stop();
        AudioManager.Instance.StopSound(SoundType.BGM, "SE_BG_Night");
        GameManager.Instance.CurrentEndingType = EndingType.Bad;
    }

    private void CheckQuestType()
    {
        _eventCamera.Priority = 100;

        if (GameManager.Instance.CurrentQuestType.Value == QuestType.Quest03)
        {
            foreach (var obj in _boarActors)
            {
                obj.gameObject.SetActive(true);
            }

            _fadeController.FadeIn(() => 
            {
                PauseManager.Instance.ExecutePause();
                EventManager.Instance.EventStart(_quest03TalkData, () =>
                {
                    StartCoroutine(StartFieldAnim());
                }); 
            });
        }
        else
        {
            GameManager.Instance.CurrentEndingType = EndingType.Normal;
            foreach (var obj in _boarActors)
            {
                obj.gameObject.SetActive(false);
            }

            _fadeController.FadeIn(() =>
            {
                PauseManager.Instance.ExecutePause();
                EventManager.Instance.EventStart(_talkData, () => 
                {
                    _keyItem.SetActive(true);
                    StartCoroutine(StartFieldAnim());
                });
            });
        }

        _keyItem.SetActive(false);
        Debug.Log($"{GameManager.Instance.CurrentQuestType}からの遷移です");
    }
    
    private void StopAudio()
    {
        _bgmCueName.ToList().ForEach(cueName => AudioManager.Instance.StopSound(SoundType.BGM, cueName));
        _seCueName.ToList().ForEach(cueName => AudioManager.Instance.StopSound(SoundType.SE, cueName));
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator StartFieldAnim()
    {
        _eventCamera.Priority = 0;
        yield return new WaitForSeconds(2f);
        GameManager.Instance.ChangeCurrentField(FieldType.Sakuya);
    }
}