using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Rigidbody2D body;
    public float scrollSpeed = 1f;

    Renderer backgroundRenderer;

    // Use this for initialization
    void Start()
    {
        backgroundRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        backgroundRenderer.material.mainTextureOffset = new Vector2(backgroundRenderer.material.mainTextureOffset.x + body.velocity.x * scrollSpeed, 0);
    }
}
