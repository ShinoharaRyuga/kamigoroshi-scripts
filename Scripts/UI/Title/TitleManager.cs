using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using CriWare;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h")] FadeController _fadeController = default;
    [SerializeField, Header("スタートボタン")] Button _startButton = default;
    [SerializeField] EventSystem _eventSystem = default;

    [SerializeField] private SynopsisController _synopsisController = default;
    [SerializeField] private float _loadTime = 5f;

    [SerializeField] private LoadingTips _tips;

    private bool _isStarted　= false;
    private void Awake()
    {
        DestroyObjectRequiredInitializing();
        _eventSystem.enabled = false;
    }

    void Start()
    {
        _isStarted = false;
        
        _fadeController.FadeIn(() =>
        {
            _eventSystem.enabled = true;
            _startButton.Select();
            Debug.Log(_eventSystem.currentSelectedGameObject.name);
            AudioManager.Instance.PlaySound(SoundType.BGM, "BGM_OP");
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            CursorController.SetVisible(true);
#endif
        }); 
    }

    public void GameStart()
    {
        if (_isStarted)
        {
            return;
        }

        _isStarted = true;
        
        AudioManager.Instance.PlaySound(SoundType.SE,"SE_Title",1f);
        
        _fadeController.FadeOut(() =>
        {
            _synopsisController.StartAnim(async () =>
            {
                await _tips.OnOpen();
                SceneManager.LoadScene("QuestSelect");
                AudioManager.Instance.StopSound(SoundType.BGM, "BGM_OP");
            });
        });
    }

    private void DestroyObjectRequiredInitializing()
    {
        foreach(var go in GameObject.FindObjectsOfType<GameObject>())
        {
            var m = go.GetComponent<IManager>();
            if(m != null)
            {
                m.DestroyObject();
            }
        }
    }

//#if UNITY_EDITOR || DEVELOPMENT_BUILD
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(2))
//        {
//            SceneManager.LoadScene("QuestSelect");
//            AudioManager.Instance.StopSound(SoundType.BGM, "BGM_OP");
//        }
//    }
//#endif
}