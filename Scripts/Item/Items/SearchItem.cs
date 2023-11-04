using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v��̃A�C�e���̈ʒu��\������A�C�e��
/// </summary>
public class SearchItem : ItemBase
{
    [Header("�p�����[�^�[")]
    [SerializeField, Tooltip("���ʎ�������")]
    public float _duration = 30f;
    [SerializeField]
    private GameObject _itemsUI;

    public override async void UseEffect()
    {
        Debug.Log("�A�C�e������");
        base.UseEffect();


        //GameObject���I���I�t����
        //�y�����v����
        _itemsUI?.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(_duration));
        _itemsUI?.SetActive(false);
    }
}
