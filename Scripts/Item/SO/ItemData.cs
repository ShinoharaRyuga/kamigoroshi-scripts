using UnityEngine;

//[TODO] 多分CSV使えるようにする

[Icon("Assets/Graphics/DataIcon/Item_Data.png")]
[CreateAssetMenu(fileName = "New ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] string _itemName = "New Item";
    [SerializeField, TextArea(5,5)] string _itemText = "None";
    [SerializeField] Sprite _itemSprite;
    [SerializeField] ItemType _itemType;
    [SerializeField] bool _isKeyItem;
    [SerializeField, Tooltip("取得後にイベントが走るかどうか")] bool _isActionEventItem;
    [SerializeField, Tooltip("取得後のイベントデータ")] EventData _eventData;


    public string ItemName => _itemName;
    public string ItemText => _itemText;
    public Sprite ItemSprite => _itemSprite;
    public ItemType ItemType => _itemType;
    public bool IsKeyItem => _isKeyItem;
    public bool IsActionEventItem => _isActionEventItem;
    public EventData EventData => _eventData;
}
