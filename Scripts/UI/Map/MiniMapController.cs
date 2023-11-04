using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] float _height = 100f;
    [SerializeField] SpriteRenderer _targetMarker;
    [Space(10)]
    [SerializeField] bool _isRotate;

    Camera _mapCam;
    Transform _targetTransform;

    void Awake()
    {
        TryGetComponent(out _mapCam);
        _targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void LateUpdate()
    {
        var targetPos = _targetTransform.position;
        targetPos.y += _height;
        _mapCam.transform.position = targetPos;

        var markerPos = _targetTransform.position;
        markerPos.y += _height / 2;
        _targetMarker.transform.position = markerPos;

        if (_isRotate)
        {
            var rot = new Vector3(90, 0, 0);
            rot.z = _targetTransform.eulerAngles.y * -1;
            _mapCam.transform.eulerAngles = rot;
        }
        else
        {
            var rot = new Vector3(90, 0, 180);
            rot.z = _targetTransform.eulerAngles.y * -1;
            _targetMarker.transform.eulerAngles = rot;
        }
    }
}
