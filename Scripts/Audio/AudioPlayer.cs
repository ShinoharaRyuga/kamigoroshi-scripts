using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private SoundType _soundType;
    [SerializeField] private string _cueName = "";
    [SerializeField] private float _volume = 1f;

    private AudioManager _audioManager;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            _audioManager = AudioManager.Instance;
        }
        
        PlaySound();
    }

    void PlaySound()
    {
        _audioManager.PlaySound(_soundType,_cueName,_volume);
    }

    public void StopSound()
    {
        _audioManager.StopSound(_soundType, _cueName);
    }
}
