using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugCommand : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current == null) return;

        if(Keyboard.current.sKey.isPressed && Keyboard.current.iKey.isPressed
            && Keyboard.current.oKey.isPressed && Keyboard.current.nKey.isPressed)
        {
            Debug.Log("�f�o�b�O�R�}���h�u���́v���Ă΂�܂����B�Q�[�����ċN�����܂��B");
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            Application.Quit();
        }
    }
}
