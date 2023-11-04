using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    [SerializeField] private GameObject _wallObject;
    [SerializeField] private GameObject _wallDissolveObject;
    [SerializeField] private CinemachineVirtualCamera _cvc;
    [SerializeField] private float _time;
    [SerializeField] private CinemachineBlendDefinition _cinemachineBlendDefinition;

    private void Start()
    {
        _wallObject.SetActive(true);
        _wallDissolveObject.SetActive(false);
    }

    public Coroutine WallDissolve()
    {
        return StartCoroutine(WallDossolveCorutine());
    }

    private IEnumerator WallDossolveCorutine()
    {
        CameraManager.Instance.ExchangeCameraPriority(_cvc, _cinemachineBlendDefinition);

        yield return new WaitForSeconds(1.5f);

        _wallObject.SetActive(false);
        _wallDissolveObject.SetActive(true);
        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Torii", 1f);

        yield return new WaitForSeconds(_time);

        CameraManager.Instance.ResetCameraPriority(_cinemachineBlendDefinition);
    }
}