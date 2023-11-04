using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Icon("Assets/Graphics/DataIcon/Event_Data.png")]
[CreateAssetMenu(fileName = "New TipsData", menuName = "ScriptableObjects/TipsData")]
public class TipsData : ScriptableObject
{
    [SerializeField] string _title = "New Title";
    [SerializeField, TextArea(3, 3)] string _contents = "New Content";

    public string Title => _title;
    public string Content => _contents;
}