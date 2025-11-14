using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class KeyRemapper : MonoBehaviour
{
    private PianoKey keyBeingRemapped = null;
    private bool waitingForKey = false;
    public static Dictionary<KeyCode, string> notaKey = new Dictionary<KeyCode, string>
    {
        {KeyCode.Tab, "C"}, {KeyCode.Q, "D"}, {KeyCode.W, "E"}, {KeyCode.E, "F"}, {KeyCode.R, "G"},
        {KeyCode.T, "A"}, {KeyCode.Y, "B"}, {KeyCode.U, "2C"}, {KeyCode.I, "2D"}, {KeyCode.O, "2E"},
        {KeyCode.P, "2F"}, {KeyCode.Delete, "2G"}, {KeyCode.End, "2A"}, {KeyCode.PageDown, "2B"},
        {KeyCode.Alpha1, "CSharp"}, {KeyCode.Alpha2, "DSharp"}, {KeyCode.Alpha3, "FSharp"}, {KeyCode.Alpha4, "GSharp"}, {KeyCode.Alpha5, "ASharp"},
        {KeyCode.Alpha6, "2CSharp"}, {KeyCode.Alpha7, "2DSharp"}, {KeyCode.Alpha8, "2FSharp"}, {KeyCode.Alpha9, "2GSharp"}, {KeyCode.Alpha0, "2ASharp"}
    };

    public static List<string> notes = new List<string>
    {
        "C", "D", "E", "F", "G", "A", "B",
        "2C", "2D", "2E", "2F", "2G", "2A", "2B",
        "CSharp", "DSharp", "ESharp", "FSharp", "GSharp",
        "2CSharp", "2DSharp", "2ESharp", "2FSharp", "2GSharp"
    };


    void Start()
    {
        aggiornaTasti();
    }
    public static void aggiornaTasti(){
        foreach (string note in notes)
        {
            if (PlayerPrefs.HasKey(note))
            {
                string keyString = PlayerPrefs.GetString(note);
                if (System.Enum.TryParse<KeyCode>(keyString, out KeyCode key))
                {
                    notaKey[key] = note;
                    Debug.Log(key.ToString() + "->" + note);
                }
            }
        }
    }
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
                    Debug.Log("VA");
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
                    PlayerPrefs.SetString(keyBeingRemapped.noteName, k.ToString());
                    Debug.Log($"{keyBeingRemapped.noteName} rimappato a {k}");
                    KeyCode oldKey = KeyCode.None;
                    foreach (var pair in notaKey)
                    {
                        if (pair.Value == keyBeingRemapped.noteName)
                        {
                            oldKey = pair.Key;
                            break;
                        }
                    }

                    if (oldKey != KeyCode.None)
                    {
                        notaKey.Remove(oldKey);
                    }
                    notaKey[k] = keyBeingRemapped.noteName;
                    waitingForKey = false;
                    keyBeingRemapped = null;
                    break;
                }
            }
        }
    }
}