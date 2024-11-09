using UnityEngine;

public class GameBoostrapper : MonoBehaviour
{
    private void Start()
    {
        LoadSceneManager.Instance.OpenAnimation();
    }
}
