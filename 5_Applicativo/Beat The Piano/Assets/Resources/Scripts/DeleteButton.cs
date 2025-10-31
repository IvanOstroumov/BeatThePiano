using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class DeleteButton : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;
    
    [Header("Zoom ratio")]
    public float scaleFactor = 1.1f;
    
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
        transform.localScale = scalaOriginale * scaleFactor; 
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = normalSprite;
        transform.localScale = scalaOriginale;
    }

    void OnMouseDown()
    {
        Debug.Log("Cancellazione salvataggio");
        PlayerPrefs.DeleteAll();
    }
}