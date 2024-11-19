using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] PanelBase panel;
    [SerializeField] RectTransform popupParent;
    [SerializeField] GameObject blocker;

    int blockCount = 0;
    int popupOpenCount = 0;
    int panelDisableCount = 0;
    Dictionary<PopupId, PopupBase> popupById = new();

    public bool HasOpenPopup => popupOpenCount > 0;
    public PanelBase Panel => panel;

    #region PANEL
    public T GetPanel<T>() where T : PanelBase
    {
        return panel as T;
    }

    public void EnablePanel()
    {
        panelDisableCount--;
        panel.EnableCanvas(panelDisableCount <= 0);
    }

    public void DisablePanel()
    {
        panelDisableCount++;
        panel.EnableCanvas(panelDisableCount <= 0);
    }
    #endregion

    #region POPUP
    public void OnPopupOpen()
    {
        popupOpenCount++;
    }

    public void OnPopupClose()
    {
        popupOpenCount--;
    }

    public bool IsPopupInstantiated(PopupId id)
    {
        return popupById.ContainsKey(id) && popupById[id] != null;
    }

    public bool IsPopupOpen(PopupId id)
    {
        return IsPopupInstantiated(id) && popupById[id].gameObject.activeSelf;
    }

    public PopupBase Popup(PopupId id)
    {
        if (!IsPopupOpen(id))
        {
            Debug.LogError($"Need to open popup first: {id}");

            return null;
        }

        return popupById[id];
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void OpenPopup(PopupId id, object args = null)
    {
        if (!IsPopupInstantiated(id))
        {
            PopupBase prefab = Resources.Load<PopupBase>("Popup" + id.ToString());
            popupById[id] = Instantiate(prefab, popupParent);
        }

        popupById[id].Open(args);
    }
    #endregion

    #region BLOCKER
    public void EnableBlocker()
    {
        blockCount++;
        blocker.SetActive(blockCount > 0);
    }

    public void DisableBlocker()
    {
        blockCount--;
        blocker.SetActive(blockCount > 0);
    }
    #endregion
}
