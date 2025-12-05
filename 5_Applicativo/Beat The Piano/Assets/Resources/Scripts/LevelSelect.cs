using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelSelect : MonoBehaviour
{
    [Header("Zoom ratio")] public float scaleFactor = 1.1f;

    private Vector3 scalaOriginale;

    [Header("Animation Objects")] public Transform top_rect;
    public Transform left_rect;
    public Transform right_rect;
    public Transform down_rect;

    private Vector3 topTarget = new Vector3(-0.45f, 4, -5);
    private Vector3 leftTarget = new Vector3(-5.7f, -0.4165f, -5);
    private Vector3 rightTarget = new Vector3(4.35f, -0.475f, -5);
    private Vector3 downTarget = new Vector3(-0.45f, -4.5f, -5);
    private AudioSource audioSource;
    private Vector3 topStart, leftStart, rightStart, downStart;

    void Start()
    {
        scalaOriginale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
        topStart = top_rect.localPosition;
        leftStart = left_rect.localPosition;
        rightStart = right_rect.localPosition;
        downStart = down_rect.localPosition;
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
        audioSource.PlayOneShot(audioSource.clip);

        StartCoroutine(PlayAnimationAndLoadScene());
    }

    //Implementato con l'aiuto di IA
    private IEnumerator PlayAnimationAndLoadScene()
    {
        float duration = audioSource.clip.length;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            top_rect.localPosition = Vector3.Lerp(topStart, topTarget, t);
            left_rect.localPosition = Vector3.Lerp(leftStart, leftTarget, t);
            right_rect.localPosition = Vector3.Lerp(rightStart, rightTarget, t);
            down_rect.localPosition = Vector3.Lerp(downStart, downTarget, t);

            yield return null;
        }

        top_rect.localPosition = topTarget;
        left_rect.localPosition = leftTarget;
        right_rect.localPosition = rightTarget;
        down_rect.localPosition = downTarget;

        //Carica la scena del gioco mettendo nei PlayerPrefs l'id del livello scelto.
        //^1 = ultimo carattere
        Debug.Log("CARICO IL LIVELLO: " + this.name[^1]);
        PlayerPrefs.SetInt("SelectedLevel", int.Parse(this.name[^1].ToString()));
        SceneManager.LoadScene("Game");
    }
}