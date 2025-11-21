using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelSelect : MonoBehaviour
{
    [Header("Zoom ratio")] public float scaleFactor = 1.1f;

    private Vector3 scalaOriginale;

    void Start()
    {
        scalaOriginale = transform.localScale;
    }

    void OnMouseEnter()
    {
        transform.localScale = scalaOriginale * scaleFactor;
    }

    void OnMouseExit()
    {
        transform.localScale = scalaOriginale;
    }

    void OnMouseDown()
    {
        //Carica la scena del gioco mettendo nei PlayerPrefs l'id del livello scelto.
        //^1 = ultimo carattere
        Debug.Log("CARICO IL LIVELLO: " + this.name[^1]);
        PlayerPrefs.SetInt("SelectedLevel", int.Parse(this.name[^1].ToString()));
        SceneManager.LoadScene("Game");
    }
}