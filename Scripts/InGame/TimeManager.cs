using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

//日本語対応
public class TimeManager : MonoBehaviour, IPause
{
    [Header("制限時間")]
    [SerializeField] float _limitTime = 0f;
    [SerializeField]
    private TextMeshProUGUI _timerText = default;

    /// <summary>
    /// それぞれのタイムアップ時の挙動
    /// </summary>
    public Action _gameOverEvent = default;

    private float _time = 0f;
    private int _minute = 0;
    private float _second = 0f;
    private bool _isPause = false;
    private bool _isPlaying = false;

    /// <summary>
    /// それぞれのクエストのStart関数かAwake関数で呼び出す
    /// </summary>
    /// <param name="callback">タイムアップ処理</param>
    public void Init(Action callback = null)
    {
        _gameOverEvent = callback;
        _time = _limitTime;
        _minute = 0;

        _timerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isPlaying) return;

        if (_isPause) return;

        _time -= Time.deltaTime;

        _minute = (int)_time / 60;
        _second = _time - _minute * 60;
        if (_time <= 0)
        {
            EndTimer();
            _gameOverEvent?.Invoke();
        }
        _timerText.text = _minute.ToString("00") + ":" + ((int)_second).ToString("00");
    }

    /// <summary>
    /// 制限時間のカウントを開始するタイミングで呼び出す関数
    /// </summary>
    public void StartTimer()
    {
        PauseManager.Instance.AddPauseObject(this);
        _isPlaying = true;
        _timerText.gameObject.SetActive(true);
    }
    public void EndTimer()
    {
        PauseManager.Instance.RemovePauseObject(this);
        _isPlaying = false;
        _timerText.gameObject.SetActive(false);
        IsEnded();
    }

    public bool IsEnded()
    {
        if(_isPlaying)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void Pause()
    {
        _isPause = true;
    }
    public void Resume()
    {
        _isPause = false;
    }
}
