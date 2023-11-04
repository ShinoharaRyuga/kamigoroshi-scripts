using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNodeGraphEditor;

[System.Serializable]
public class PlayAnimation : ActionNode
{
    public string _animationName = "";
    public Animator _animator;
    protected override void OnStart()
    {
        _animator = context.animator;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (_animator == null || _animationName == "")
        {
            return State.Failure;
        }
        
        _animator.Play(_animationName);
        return State.Success;
    }
}
