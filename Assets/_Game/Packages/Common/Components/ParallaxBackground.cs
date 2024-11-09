using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Vector2 speed = Vector2.one;

    void Start()
    {
        image.material = new Material(image.material);
    }

    void Update()
    {
        image.material.mainTextureOffset += speed * Time.deltaTime;
    }
}
