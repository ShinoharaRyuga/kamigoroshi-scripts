using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>�|�[�Y�֘A�̏������s���N���X </summary>
public class PauseManager : MonoBehaviour, IManager
{
    static PauseManager _instance;

    static public PauseManager Instance => _instance;

    /// <summary>�|�[�Y���s���I�u�W�F�N�g�̔z�� </summary>
    List<IPause> _pauseObjects = new List<IPause>();

    /// <summary>�|�[�Y�� </summary>
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

    /// <summary>�|�[�Y�E�ĊJ�̏������s�� </summary>
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

    /// <summary>�|�[�Y��������I�u�W�F�N�g�����X�g�ɒǉ����� </summary>
    public void AddPauseObject(IPause obj)
    {
        _pauseObjects.Add(obj);
    }

    /// <summary>�|�[�Y��������I�u�W�F�N�g�����X�g����폜���� </summary>
    public void RemovePauseObject(IPause obj)
    {
        _pauseObjects.Remove(obj);
    }

    /// <summary>�|�[�Y</summary>
    void Pause()
    {
        foreach (var obj in _pauseObjects)
        {
            obj.Pause();
        }
    }

    /// <summary>�|�[�Y���� </summary>
    void Resume()
    {
        foreach (var obj in _pauseObjects)
        {
            obj.Resume();
        }
    }

    /// <summary>�^�C�g���V�[���ɑJ�ڎ��ɃI�u�W�F�N�g��S�Ĕj������ </summary>
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
