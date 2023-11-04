using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SelectionWindowView : MonoBehaviour
{
    [SerializeField] Selectable _firstSelection;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] bool _isViewSwitchEnabledOnForward = false;
    [SerializeField] bool _isViewSwitchEnabledOnBackward = true;
    [Space(10)]
    [SerializeField] Transform _childrenRoot;
    [Space(10)]
    [SerializeField] protected SelectionWindowView[] _nextWindows;

    protected GameObject[] _children;
    Selectable _first;

    public delegate void SetFirstSelectionEvent(GameObject obj);

    protected IUIResetView _resetView;

    public bool IsFirstSelected => EventSystem.current.currentSelectedGameObject == _firstSelection;
    public bool IsViewSwitchEnabledOnForward => _isViewSwitchEnabledOnForward;
    public bool IsViewSwitchEnabledOnBackward => _isViewSwitchEnabledOnBackward;
    public GameObject[] Children => _children;

    private void Awake()
    {
        _first = _firstSelection;

        GetChild();
        ActiveWindow(false);
        ChangeAlpha(false);

        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    void GetChild()
    {
        if (_childrenRoot != null)
        {
            var count = _childrenRoot.childCount;
            _children = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                _children[i] = _childrenRoot.GetChild(i).gameObject;
            }
        }
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }

    private void Reset()
    {
        TryGetComponent(out _canvasGroup);
    }

    /// <summary>
    /// ウィンドウの表示を切り替える
    /// </summary>
    /// <param name="flag"></param>
    public void ActiveWindow(bool flag)
    {
        _canvasGroup.interactable = flag;
        _canvasGroup.blocksRaycasts = flag;
        _canvasGroup.ignoreParentGroups = flag;
    }

    public void ChangeAlpha(bool flag)
    {
        _canvasGroup.alpha = flag ? 1 : 0;
    }

    public void SetFirstSelection()
    {
        if (!_firstSelection) return;

        var check = _firstSelection.interactable;

        if(!check)
        {
            _firstSelection = _first;
        }

        UIManager.Instance.UpdatePreviousSelection(_firstSelection.gameObject);
        _firstSelection.Select();
    }

    public void ChangeSelection(Selectable selectable)
    {
        _firstSelection = selectable;
    }

    public void ResetWindow()
    {
        ChangeSelection(_first);

        if(_resetView != null)
        {
            _resetView.ResetView();
        }
    }

    /// <summary>
    /// 有効なUIを設定されているUIに切り替える
    /// </summary>
    public void OnSwitchToValidWindow()
    {
        if (!_firstSelection) return;

        GetChild();

        SetFirstSelection();
        UIManager.Instance.UpdateCurrentWindow(this);
    }
}
