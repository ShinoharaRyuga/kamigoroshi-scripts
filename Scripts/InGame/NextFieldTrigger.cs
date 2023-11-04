using UnityEngine;
using System;
using System.Linq;

/// <summary>第二フィールドに遷移処理を実行するクラス </summary>
public class NextFieldTrigger : MonoBehaviour
{
    [SerializeField] private string[] _bgmCueName;
    [SerializeField] private string[] _seCueName;
    /// <summary>遷移処理 </summary>
    event Action _onChangeScene = default;

    /// <summary>遷移処理 </summary>
    public event Action OnChangeScene
    {
        add { _onChangeScene += value; }
        remove { _onChangeScene -= value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _onChangeScene?.Invoke();
            StopAudio();
        }
    }

    private void StopAudio()
    {
        _bgmCueName.ToList().ForEach(cueName => AudioManager.Instance.StopSound(SoundType.BGM, cueName));
        _seCueName.ToList().ForEach(cueName => AudioManager.Instance.StopSound(SoundType.SE, cueName));
    }
}
