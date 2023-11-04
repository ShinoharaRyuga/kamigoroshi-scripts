using UnityEngine;

[System.Serializable]
public class FieldInfo
{
    [SerializeField, Header("桜河街")]
    FieldModel _field01 = default;
    [SerializeField, Header("サクラ神社")]
    FieldModel _field02 = default;

    /// <summary>フィールド名のイラスト </summary>
    public FieldModel[] FieldModels => new FieldModel[] { _field01, _field02 };
}

[System.Serializable]
public class FieldModel
{
    [SerializeField, Header("フィールド名のイラスト")] Sprite _fieldName;
    [SerializeField, Header("マップのイラスト")] Sprite _fieldMap;

    public Sprite FieldMap => _fieldMap;
    public Sprite FieldName => _fieldName;
}