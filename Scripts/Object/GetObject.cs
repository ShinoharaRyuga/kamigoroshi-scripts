using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputControl;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public partial class GetObject : TargetChecker
{
    private Transform _thisTransform;
    private List<FieldObject> _fieldObjects = new();

    public FieldObject FieldObject => _fieldObjects.Count > 0 ? _fieldObjects[0] : null;

    private void Awake()
    {
        TryGetComponent(out _thisTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out FieldObject obj)) return;

        _fieldObjects.Add(obj);

        if (!EventManager.Instance.IsPlaying)
        {
            obj.Enter();
        }

        if (_fieldObjects.Count > 0)
        {
            _isEnable.Value = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out FieldObject obj)) return;

        _fieldObjects.Remove(obj);
        obj.Exit();

        if (_fieldObjects.Count <= 0)
        {
            _isEnable.Value = false;
        }
    }

    public override bool OnCheck()
    {
        if (_fieldObjects.Count <= 0) return false;

        return true;
    }

    public override void OnFuncCall()
    {
        if (_fieldObjects.Count <= 0) return;

        FieldObject target = default;
        var minDis = 100f;

        foreach (var obj in _fieldObjects)
        {
            var dis = Vector2.Distance(_thisTransform.position, obj.transform.position);

            if (dis >= minDis) continue;

            minDis = dis;
            target = obj;
        }
        
        AudioManager.Instance.PlaySound(SoundType.SE,"SE_Select",1f);

        target.Talk();
    }
}