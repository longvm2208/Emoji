using UnityEngine;

public class HomeBoostrapper : MonoBehaviour
{
    private void Start()
    {
        LoadSceneManager.Instance.OpenAnimation();
    }
}
