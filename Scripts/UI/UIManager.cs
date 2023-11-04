using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using InputControl;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager: MonoBehaviour, IManager
{
    static UIManager _instance;

    GameObject _prevSelection;
    HeaderButtonView _prevHeader;

    SelectionWindowView _currentWindowView;
    SelectionWindowView _prevWindowView;
    List<SelectionWindowView> _selectionWindowStacks = new List<SelectionWindowView>();

    bool _isNavigatingBack;

    public static UIManager Instance => _instance;
    public GameObject PrevSelection => _prevSelection;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(CheckUISelected());
        PlayerInputs.Instance.AddUIAction(UIActionType.Cancel, UnityEngine.InputSystem.InputActionPhase.Performed, SwitchToPreviousWindow);

        SceneManager.sceneUnloaded += _ => _selectionWindowStacks.Clear();
    }

    IEnumerator CheckUISelected()
    {
        while (true)
        {
            yield return new WaitUntil(() => 
            (EventSystem.current != null) && 
            (EventSystem.current.currentSelectedGameObject != _prevSelection)
            );

            // まだオブジェクトを未選択、または許可リストを選択しているなら何もしない
            if ((_prevSelection == null) || _currentWindowView.Children.Contains(EventSystem.current.currentSelectedGameObject))
            {
                continue;
            }

            // 選択しているものがなくなった、または許可していない Selectable を選択した場合は前の選択に戻す
            EventSystem.current.SetSelectedGameObject(_prevSelection);
        }
    }

    public void SwitchToPreviousWindow()
    {
        if (_selectionWindowStacks.Count <= 1) return;

        _isNavigatingBack = true;

        var index = _selectionWindowStacks.Count - 1;
        _selectionWindowStacks.RemoveAt(index);

        var next = _selectionWindowStacks[index - 1];
        next.OnSwitchToValidWindow();
    }

    void UpdatePreviousWindow()
    {
        if (!_currentWindowView) return;

        //if (_selectionWindowStacks.Count == 1) //スタックに入っているのがHeaderだけだったら
        //{
        //    if (_prevWindowView)
        //    {
        //        _prevWindowView.ChangeAlpha(false);
        //    }
        //}

        _prevWindowView = _currentWindowView;
        _prevWindowView.ActiveWindow(false);

        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Enter");

        if (_isNavigatingBack) return;

        if (_prevWindowView.IsViewSwitchEnabledOnForward) //進む時に前のウィンドウが表示を切り替えることを許可していたら
        {
            _prevWindowView.ChangeAlpha(false);
        }
    }

    /// <summary>
    /// 現在選択されているウィンドウを更新する
    /// </summary>
    /// <param name="window"></param>
    public void UpdateCurrentWindow(SelectionWindowView window)
    {
        //1つ前に選択されていたウインドウを更新する
        UpdatePreviousWindow();

        if (_isNavigatingBack)
        {
            _isNavigatingBack = false;

            if (_prevWindowView)
            {
                if (_prevWindowView != window) //次のウィンドウと前のウィンドウが違う時だけ
                {
                    //_prevWindowView.ActiveWindow(false);

                    if (_prevWindowView.IsViewSwitchEnabledOnBackward) //戻る時に前のウィンドウが表示を切り替えることを許可していれば
                    {
                        _prevWindowView.ChangeAlpha(false);
                    }

                    _prevWindowView.ResetWindow();
                }
            }
        }
        else //ウィンドウを進んでいる時だけ追加する
        {
            //window.SetFirstSelection();
            _selectionWindowStacks.Add(window);
        }

        _currentWindowView = window;
        _currentWindowView.ActiveWindow(true);
        _currentWindowView.ChangeAlpha(true);
    }

    /// <summary>
    /// 選択されているUIを更新する
    /// </summary>
    /// <param name="obj"></param>
    public void UpdatePreviousSelection(GameObject obj)
    {
        _prevSelection = obj;
    }

    /// <summary>
    /// 全てのウインドウをリセットさせる
    /// </summary>
    public void ResetAllWindows(SelectionWindowView view)
    {
        for (int i = 0; i < _selectionWindowStacks.Count; i++)
        {
            SwitchToPreviousWindow();
        }

        _isNavigatingBack = false;

        view.ActiveWindow(false);
        view.ChangeAlpha(false);
    }

    /// <summary>
    /// ホーム画面の最初の画面を表示する
    /// </summary>
    /// <param name="view"></param>
    /// <param name="first"></param>
    public void EnableFirstWindow(SelectionWindowView view, Selectable first)
    {
        view.ActiveWindow(true);
        view.ChangeAlpha(true);

        view.ChangeSelection(first);
        view.SetFirstSelection();
    }

    /// <summary>
    /// 開始時にヘッダー画面を設定する
    /// </summary>
    /// <param name="view"></param>
    public void SetHeaderView(SelectionWindowView view)
    {
        view.OnSwitchToValidWindow();
    }

    /// <summary>
    /// 1つ前に選択されていたヘッダーボタンを更新する
    /// </summary>
    /// <param name="header"></param>
    public void UpdatePrevHeader(HeaderButtonView header)
    {
        if(_prevHeader)
        {
            _prevHeader.OnDeselect();
        }

        _prevHeader = header;
    }

    public void DestroyObject()
    {
        _instance = null;
        Destroy(gameObject);
    }
}
