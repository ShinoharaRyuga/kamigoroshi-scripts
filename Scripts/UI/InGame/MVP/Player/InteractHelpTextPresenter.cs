using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InteractHelpTextPresenter : MonoBehaviour
{
    [SerializeField] InteractHelpTextView _getItemText;
    [SerializeField] InteractHelpTextView _talkText;
    [SerializeField] InteractHelpTextView _gerObjectText;

    private void Start()
    {
        var p = PlayerManager.Instance;
        var isDebug = p ? false : true;

        if (isDebug) return;

        if (_getItemText)
        {
            p.PlayerGetItem.IsEnable.Subscribe(_getItemText.SetActiveText).AddTo(this);
        }

        if (_talkText)
        {
            p.PlayerActorTalker.IsEnable.Subscribe(_talkText.SetActiveText).AddTo(this);
        }

        if (_gerObjectText)
        {
            p.PlayerGetObject.IsEnable.Subscribe(_gerObjectText.SetActiveText).AddTo(this);
        }
    }
}
