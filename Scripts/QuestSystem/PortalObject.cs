using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using Unity.VisualScripting;
using UnityEngine;

public class PortalObject : MonoBehaviour
{
    [SerializeField] private Quest01Manager _questManager;
    [SerializeField] private CriAtomSource _criAtomSource;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            _criAtomSource.Stop();
            _questManager.OnQuestStart();
        }
    }
}