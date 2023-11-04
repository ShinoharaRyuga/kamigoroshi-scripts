using UnityEngine;

/// <summary>�A�v���P�[�V�������I�������� </summary>
public class AppQuit : MonoBehaviour
{
    [SerializeField, Header("�V�[������FadeCanvas���A�^�b�`")]
    FadeController _fadeController = default;

    /// <summary>
    /// �A�v���P�[�V�����𗎂Ƃ� 
    /// OnClick�ŌĂяo��
    /// </summary>
    public void Quit()
    {
        _fadeController.FadeOut(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
