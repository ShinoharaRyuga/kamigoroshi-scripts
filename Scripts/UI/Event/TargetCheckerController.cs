using InputControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetCheckerController : MonoBehaviour
{
    [SerializeField] ActorTalker _actorTalker;
    [SerializeField] GetObject _getObject;

    int _inputID;

    private void Start()
    {
        _inputID = PlayerInputs.Instance.AddInGameAction(ActionType.Submit, InputActionPhase.Started, OnCheck);
    }

    void OnCheck()
    {
        var obj = _getObject.OnCheck();
        var actor = _actorTalker.OnCheck();

        if(actor)
        {
            _actorTalker.OnFuncCall();
        }
        else if(obj)
        {
            _getObject.OnFuncCall();
        }
    }

    public void OnRemoveInput()
    {
        PlayerInputs.Instance.RemoveInGameAction(_inputID);
    }
}
