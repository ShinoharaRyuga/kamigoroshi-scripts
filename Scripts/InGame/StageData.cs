using UnityEngine;

[System.Serializable]
public class FieldInfo
{
    [SerializeField, Header("���͊X")]
    FieldModel _field01 = default;
    [SerializeField, Header("�T�N���_��")]
    FieldModel _field02 = default;

    /// <summary>�t�B�[���h���̃C���X�g </summary>
    public FieldModel[] FieldModels => new FieldModel[] { _field01, _field02 };
}

[System.Serializable]
public class FieldModel
{
    [SerializeField, Header("�t�B�[���h���̃C���X�g")] Sprite _fieldName;
    [SerializeField, Header("�}�b�v�̃C���X�g")] Sprite _fieldMap;

    public Sprite FieldMap => _fieldMap;
    public Sprite FieldName => _fieldName;
}