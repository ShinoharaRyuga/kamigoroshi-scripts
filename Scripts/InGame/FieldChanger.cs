using UnityEngine;
using System;

/// <summary>�V�[���J�ڂ��s���N���X </summary>
public class FieldChanger : MonoBehaviour
{
    [SerializeField, Header("�V�[�����ɂ�����̂��A�^�b�`���Ă�������")]
    FadeController _fadeController = default;
   
    /// <summary>�t�B�[���h�J�ڂ��s�� </summary
    /// <param name="type">�J�ڎ��</param>
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