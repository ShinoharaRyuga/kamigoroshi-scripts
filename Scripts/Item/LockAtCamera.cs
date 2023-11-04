using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAtCamera : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーカメラ")]
    private Transform _playerCameraTr;
    private Transform _thisTr;

    private void Start()
    {
        _thisTr = this.transform;
    }

    private void Update()
    {
        if(_playerCameraTr !=　null)
        {
            _thisTr.LookAt(2 * _thisTr.position - _playerCameraTr.position);
        }
    }
}
