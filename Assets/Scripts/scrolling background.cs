using UnityEngine;

public class scrollingbackground : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private Renderer bgrenderer;

    void Update()
    {
        bgrenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
