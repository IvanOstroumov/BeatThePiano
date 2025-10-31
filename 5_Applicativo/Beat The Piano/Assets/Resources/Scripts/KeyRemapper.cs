using UnityEngine;
using System;

public class KeyRemapper : MonoBehaviour
{
    private PianoKey keyBeingRemapped = null;
    private bool waitingForKey = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !waitingForKey)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                PianoKey pk = hit.collider.GetComponent<PianoKey>();
                if (pk != null)
                {
                    keyBeingRemapped = pk;
                    waitingForKey = true;
                    Debug.Log($"Cliccato {pk.noteName}. Premi un tasto per rimappare!");
                }
            }
        }
        
        if (waitingForKey && Input.anyKeyDown)
        {
            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k) && k != KeyCode.Mouse0)
                {
                    keyBeingRemapped.key = k;
                    PlayerPrefs.SetString("key_" + keyBeingRemapped.noteName, k.ToString());
                    Debug.Log($"{keyBeingRemapped.noteName} rimappato a {k}");
                    waitingForKey = false;
                    keyBeingRemapped = null;
                    break;
                }
            }
        }
    }
}