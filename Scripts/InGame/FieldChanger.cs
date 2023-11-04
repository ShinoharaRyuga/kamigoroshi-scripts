using UnityEngine;
using System;

/// <summary>シーン遷移を行うクラス </summary>
public class FieldChanger : MonoBehaviour
{
    [SerializeField, Header("シーン内にあるものをアタッチしてください")]
    FadeController _fadeController = default;
   
    /// <summary>フィールド遷移を行う </summary
    /// <param name="type">遷移種類</param>
    public void FieldChange(SceneTransitionType type, Action action = null)
    {
        if (type == SceneTransitionType.Enter)
        {
            _fadeController.FadeIn(action);
            
        }
        else
        {
            _fadeController.FadeOut(action);
        }
    }
}