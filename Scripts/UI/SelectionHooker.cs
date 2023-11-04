using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionHooker : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    /// <summary>
    /// 選択解除時にそれまで選択されていたオブジェクトを覚えておく。
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        GameObject obj = null;

        if(eventData.selectedObject.TryGetComponent(out Selectable selectable))
        {
            if(selectable.interactable)
            {
                obj = eventData.selectedObject;
            }
        }

        UIManager.Instance.UpdatePreviousSelection(obj);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (UIManager.Instance.PrevSelection == this.gameObject) return;

        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Select");
    }
}