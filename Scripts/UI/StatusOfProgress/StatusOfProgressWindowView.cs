using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusOfProgressWindowView : SelectionWindowView
{
    MissionButtonView[] _missionButtons;

    protected override void OnAwake()
    {
        _missionButtons = new MissionButtonView[_children.Length];
        var index = 0;

        for (int i = 0; i < _children.Length; i++)
        {
            if (_children[i].TryGetComponent(out MissionButtonView button))
            {
                _missionButtons[i] = button;
                button.Setup(ChangeSelection, _nextWindows[index]);

                if (index < _nextWindows.Length - 1)
                {
                    index++;
                }
            }
        }
    }
}
