using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderWindowView : SelectionWindowView
{
    HeaderButtonView[] _headerButtons;

    protected override void OnAwake()
    {
        _headerButtons = new HeaderButtonView[_children.Length];
        var index = 0;

        for (int i = 0; i < _children.Length; i++)
        {
            if (_children[i].TryGetComponent(out HeaderButtonView button))
            {
                _headerButtons[i] = button;
                button.Setup(() =>
                {
                    if (button.TryGetComponent(out Button bt))
                    {
                        ChangeSelection(bt);
                    }
                }, _nextWindows[index]);

                if (index < _nextWindows.Length - 1)
                {
                    index++;
                }
            }
        }
    }
}
