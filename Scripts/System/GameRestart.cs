using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

public class GameRestart : MonoBehaviour
{
    [SerializeField] private FadeController _fadeController;

    private bool _isRestart = false;

    private void Start()
    {
        _isRestart = false;
    }
    public void Restart()
    {
        if (!_isRestart)
        {
            _isRestart = true;
            AsyncRestart();
        }
    }
    public async UniTask AsyncRestart()
    {
        await _fadeController.FadeOut();

        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }
}