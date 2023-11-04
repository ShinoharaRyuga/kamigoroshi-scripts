using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerAttachment : MonoBehaviour
{
    CinemachineVirtualCameraBase _main;
    [SerializeField] CinemachineVirtualCameraBase _rightMiddle, _rightFront, _rightBack;

    public CinemachineVirtualCameraBase Main => _main;
    /// <summary>
    /// 右中・右前・右後
    /// </summary>
    public CinemachineVirtualCameraBase[] Cameras => new CinemachineVirtualCameraBase[] { _rightMiddle, _rightFront, _rightBack };

    const string PLAYER_TAG = "Player";

    private void Start()
    {
        _main = PlayerManager.Instance.GetComponentInChildren<CinemachineVirtualCameraBase>();
        var p = GameObject.FindGameObjectWithTag(PLAYER_TAG);

        if(p)
        {
            foreach (var cam in Cameras)
            {
                if (cam.TryGetComponent(out CinemachineVirtualCamera vcam))
                {
                    vcam.m_Follow = p.transform;
                    vcam.m_LookAt = p.transform;
                }
            }
        }

        CameraManager.Instance.Initialize(this);
    }
}
