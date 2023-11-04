using UnityEngine;

/// <summary>アプリケーションを終了させる </summary>
public class AppQuit : MonoBehaviour
{
    [SerializeField, Header("シーン内のFadeCanvasをアタッチ")]
    FadeController _fadeController = default;

    /// <summary>
    /// アプリケーションを落とす 
    /// OnClickで呼び出し
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
