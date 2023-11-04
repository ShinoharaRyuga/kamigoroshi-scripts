using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>ポーズ関連の処理を行うクラス </summary>
public class PauseManager : MonoBehaviour, IManager
{
    static PauseManager _instance;

    static public PauseManager Instance => _instance;

    /// <summary>ポーズを行うオブジェクトの配列 </summary>
    List<IPause> _pauseObjects = new List<IPause>();

    /// <summary>ポーズ中 </summary>
    bool _isPause = false;
    public bool IsPause => _isPause;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = GetComponent<PauseManager>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneUnloaded += ClearPauseObjects;
        }
    }

    /// <summary>ポーズ・再開の処理を行う </summary>
    public void ExecutePause()
    {
        if (!_isPause)
        {
            Pause();
        }
        else
        {
            Resume();
        }

        _isPause = !_isPause;
    }

    /// <summary>ポーズをさせるオブジェクトをリストに追加する </summary>
    public void AddPauseObject(IPause obj)
    {
        _pauseObjects.Add(obj);
    }

    /// <summary>ポーズをさせるオブジェクトをリストから削除する </summary>
    public void RemovePauseObject(IPause obj)
    {
        _pauseObjects.Remove(obj);
    }

    /// <summary>ポーズ</summary>
    void Pause()
    {
        foreach (var obj in _pauseObjects)
        {
            obj.Pause();
        }
    }

    /// <summary>ポーズ解除 </summary>
    void Resume()
    {
        foreach (var obj in _pauseObjects)
        {
            obj.Resume();
        }
    }

    /// <summary>タイトルシーンに遷移時にオブジェクトを全て破棄する </summary>
    void ClearPauseObjects(Scene unloadScene)
    {
        if (unloadScene.name == "Field02")
        {
            _pauseObjects.Clear();
        }
    }

    public void DestroyObject()
    {
        _instance = null;
        Destroy(this.gameObject);
    }
}
