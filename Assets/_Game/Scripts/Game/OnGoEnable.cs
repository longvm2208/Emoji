using UnityEngine;
using UnityEngine.Events;

public class OnGoEnable : MonoBehaviour
{
    [SerializeField] UnityEvent onEnable;

    public UnityEvent OnEnableUE => onEnable;

    void OnEnable()
    {
        if (DataManager.Instance.GameData.SelectedLevelIndex > 0) return;
        onEnable?.Invoke();
    }
}
