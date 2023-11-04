using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Icon("Assets/Graphics/DataIcon/Mission_Data.png")]
[CreateAssetMenu(fileName = "New MissionData", menuName = "ScriptableObjects/MissionData")]
public class MissionData : ScriptableObject
{
    [SerializeField, Header("表示する背景の画像")] Sprite _backGroundSprite = default; 
    [SerializeField] string _title = "New Title";
    [SerializeField, TextArea(5,5)] string _details = "New Details";
    [SerializeField, TextArea(5,5)] string[] _contents = new string[1] {"New Content"};

    /// <summary>表示する背景の画像 </summary>
    public Sprite BackGroundSprite => _backGroundSprite;
    public string Title => _title;
    public string Details => _details;
    public string[] Contents => _contents;
}
