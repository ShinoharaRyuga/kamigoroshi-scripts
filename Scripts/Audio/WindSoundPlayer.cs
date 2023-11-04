using System.Collections;
using System.Collections.Generic;
using CriWare;
using UnityEngine;

public class WindSoundPlayer : MonoBehaviour
{
    [SerializeField, Header("AtomSource")]
    CriAtomSource _criAtomSource = default;

    private void Start()
    {
        AudioManager.Instance.AddAtomSource(SoundType.SE, _criAtomSource, _criAtomSource.volume);
    }
}
