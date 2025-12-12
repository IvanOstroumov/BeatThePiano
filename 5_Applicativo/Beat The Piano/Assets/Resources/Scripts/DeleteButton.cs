using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class DeleteButton : MonoBehaviour
{
    [Header("Sprites")] public Sprite normalSprite;
    public Sprite hoverSprite;

    [Header("Zoom ratio")] public float scaleFactor = 1.1f;

    [Header("What to delete?")] [Description("0: All, 1: KeyMap")]
    public int select;

    private List<string> notes;
    private Dictionary<KeyCode, string> notaKey;

    private SpriteRenderer spriteRenderer;
    private Vector3 scalaOriginale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
        scalaOriginale = transform.localScale;
        notes = KeyRemapper.notes;
        notaKey = KeyRemapper.notaKey;
        
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
        if (select == 0)
        {
            Debug.Log("Cancellazione salvataggio");
            PlayerPrefs.DeleteAll();
            KeyRemapper.notaKey = KeyRemapper.defaultNotaKey;
            ScreenKeyLoader.load();
        } else if (select == 1)
        {
            Debug.Log("Cancellazione tasti");
            KeyRemapper.notaKey = KeyRemapper.defaultNotaKey;
            foreach (string note in notes)
            {
                if (PlayerPrefs.HasKey(note))
                {
                    string keyString = PlayerPrefs.GetString(note);

                    //Prova a trasformare il contenuto della PlayerPref in un tasto KeyCode, se riesce lo cancella
                    if (System.Enum.TryParse<KeyCode>(keyString, out KeyCode key))
                    {
                        PlayerPrefs.DeleteKey(note);
                    }
                }
            }
            ScreenKeyLoader.load();
        }
    }
}