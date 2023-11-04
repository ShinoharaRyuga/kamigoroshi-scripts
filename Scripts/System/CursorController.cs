using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>カーソルを操作するクラス </summary>
public class CursorController : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame && Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    /// <summary>カーソルの表示/非表示を操作</summary>
    public static void SetVisible(bool visible)
    {
        if(Gamepad.current != null) { return; }

        Cursor.visible = visible;
        Cursor.lockState = (visible) ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
