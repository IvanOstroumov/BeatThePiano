using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class buttonHover : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;

    [Header("Scene to Load")]
    public string sceneName; 
    
    private SpriteRenderer spriteRenderer;
    private Vector3 scalaOriginale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
        scalaOriginale = transform.localScale;
    }

    void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
        transform.localScale = scalaOriginale * 1.1f; 
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = normalSprite;
        transform.localScale = scalaOriginale;
    }

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name not set for " + gameObject.name);
        }
    }
}