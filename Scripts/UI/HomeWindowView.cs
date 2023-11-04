using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputControl;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class HomeWindowView : MonoBehaviour
{
    [SerializeField] CanvasGroup _homeWindow;
    [Space(10)]
    [SerializeField] SelectionWindowView _headerWindow;
    [SerializeField] Selectable _firstTabSelectable;
    [Space(10)]
    [SerializeField] ItemWindowView _itemWindow;
    [Space(10)]
    [SerializeField] bool _includeParent;

    CanvasGroup[] _children;
    bool _isActived = true;
    int[] _inputID = new int[2];

    private void Awake()
    {
        _children = GetChildrenRecursive(this.transform, _includeParent);
    }

    private void Start()
    {
        _inputID[0] = PlayerInputs.Instance.AddInGameAction(ActionType.Pause, UnityEngine.InputSystem.InputActionPhase.Started, () =>
        {
            var isDebug = PauseManager.Instance ? false : true;

            if (!isDebug)
            {
                PauseManager.Instance.ExecutePause();
            }
            SetWindow();
        });

        _inputID[1] = PlayerInputs.Instance.AddUIAction(UIActionType.Pause, UnityEngine.InputSystem.InputActionPhase.Performed, () =>
        {
            var isDebug = PauseManager.Instance ? false : true;

            if (!isDebug)
            {
                PauseManager.Instance.ExecutePause();
            }
            SetWindow(); 
        });

        UIManager.Instance.SetHeaderView(_headerWindow);

        SetWindow();

        SceneManager.sceneUnloaded += OnRemoveInputs;
    }

    void OnRemoveInputs(Scene thisScene)
    {
        PlayerInputs.Instance.RemoveInGameAction(_inputID[0]);
        PlayerInputs.Instance.RemoveUIAction(_inputID[1]);
    }

    private void Reset()
    {
        TryGetComponent(out _homeWindow);
    }

    void SetWindow()
    {
        _isActived = !_isActived;

        _homeWindow.alpha = _isActived ? 1 : 0;

        _homeWindow.interactable = _isActived;
        _homeWindow.blocksRaycasts = _isActived;

        foreach (var child in _children)
        {
            child.ignoreParentGroups = _isActived;
        }

        if (!_isActived)
        {
            UIManager.Instance.ResetAllWindows(_headerWindow);
            PlayerInputs.Instance.ChangeActionMap(ActionMaps.InGame);
        }
        else
        {
            _itemWindow.OnUpdateView();
            UIManager.Instance.EnableFirstWindow(_headerWindow, _firstTabSelectable);
            PlayerInputs.Instance.ChangeActionMap(ActionMaps.UI);
        }
    }

    CanvasGroup[] GetChildrenRecursive(Transform parent, bool includeParent = true)
    {
        var parentAndChildren = parent.GetComponentsInChildren<CanvasGroup>(true);

        if (includeParent)
        {
            return parentAndChildren;
        }

        var children = new CanvasGroup[parentAndChildren.Length - 1];

        // 親を除く子オブジェクトを結果にコピー
        Array.Copy(parentAndChildren, 1, children, 0, children.Length);

        return children;
    }
}
