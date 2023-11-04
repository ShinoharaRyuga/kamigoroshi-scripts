using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�C�e���̊��N���X
/// �e�L�X�g�f�[�^��e��p�����[�^�[�A�g�p���̊֐��Ȃǂ�����
/// </summary>
public abstract class ItemBase : MonoBehaviour 
{
    [SerializeField, Tooltip("�A�C�e���̃f�[�^")]
    private ItemData _itemTextData;
    [SerializeField, Min(0)]
    private int _quantity = 0;
    [SerializeField]
    private bool _isFirstTimeGetting = true;

    public ItemData ItemData => _itemTextData; 
    public int Quantity => _quantity;
    public bool IsFirstTimeGetting => _isFirstTimeGetting;

    public virtual void UseEffect()
    {
        _quantity--;
    }

    public virtual void Add()
    {
        if(_isFirstTimeGetting)
        {
            _isFirstTimeGetting = false;
        }

        _quantity++;
    }

    public void Dispose()
    {
        _quantity--;
    }
}
