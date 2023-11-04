using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCameraBase _mainCam;
    CinemachineVirtualCameraBase _rightMiddle, _rightFront, _rightBack;
    CinemachineVirtualCamera _targetCamera;

    CinemachineBrain _brain;
    bool _isCameraMoved;

    static CameraManager _instance;
    public static CameraManager Instance => _instance;
    public bool IsCameraMoved => _isCameraMoved;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Initialize(CameraManagerAttachment attachment)
    {
        if (Camera.main.TryGetComponent(out CinemachineBrain brain))
        {
            _brain = brain;
        }

        _mainCam = attachment.Main;
        _mainCam.MoveToTopOfPrioritySubqueue();

        _rightMiddle = attachment.Cameras[0];
        _rightFront = attachment.Cameras[1];
        _rightBack = attachment.Cameras[2];
    }

    public void SetFlag()
    {
        _isCameraMoved = true;
    }

    public void ExchangeCutSceneCamera(CutSceneCameraPosition type, CinemachineBlendDefinition definition)
    {
        CinemachineVirtualCameraBase cam = default;

        switch (type)
        {
            case CutSceneCameraPosition.RightMiddle:
                cam = _rightMiddle;
                break;
            case CutSceneCameraPosition.RightFront:
                cam = _rightFront;
                break;
            case CutSceneCameraPosition.RightBack:
                cam = _rightBack;
                break;
            default:
                Debug.LogError("未定義のTypeが選ばれました");
                return;
        }

        ExchangeCameraPriority(cam, definition);
    }

    public void IntaractCamera(CinemachineBlendDefinition definition)
    {
        ExchangeCameraPriority(_targetCamera, definition);
    }

    public void ExchangeCameraPriority(CinemachineVirtualCameraBase camera, CinemachineBlendDefinition definition = default)
    {
        SetFlag();
        _brain.m_DefaultBlend = definition;

        camera.MoveToTopOfPrioritySubqueue();
    }

    public void ResetCameraPriority(CinemachineBlendDefinition definition)
    {
        ExchangeCameraPriority(_mainCam, definition);
        _isCameraMoved = false;
    }

    public void SetTargetCamera(CinemachineVirtualCamera cam)
    {
        _targetCamera = cam;
    }
}
public enum CutSceneCameraPosition
{
    RightMiddle,
    RightFront,
    RightBack,
}
