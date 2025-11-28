using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Questo script prende il dizionario salvato in KeyRemapper dell' associazione tra tasti e note, e scrive
//sotto ai tasti del pianoforte quale tasto della tastiera bisogna schiacciare
public class ScreenKeyLoader : MonoBehaviour
{
    public static HashSet<KeyCode> keysWithLongNames = new HashSet<KeyCode>();

    void Start()
    {
        keysWithLongNames.Add(KeyCode.Alpha0);
        keysWithLongNames.Add(KeyCode.Alpha1);
        keysWithLongNames.Add(KeyCode.Alpha2);
        keysWithLongNames.Add(KeyCode.Alpha3);
        keysWithLongNames.Add(KeyCode.Alpha4);
        keysWithLongNames.Add(KeyCode.Alpha5);
        keysWithLongNames.Add(KeyCode.Alpha6);
        keysWithLongNames.Add(KeyCode.Alpha7);
        keysWithLongNames.Add(KeyCode.Alpha8);
        keysWithLongNames.Add(KeyCode.Alpha9);
        load();
    }

    public static void load()
    {
        foreach (var i in KeyRemapper.notaKey)
        {
            //Si prende il gameObject chiamato col nome della nota + _Text e si trova la sua componente text che poi si va a modificare
            GameObject qol = GameObject.Find(i.Value + "_Text");
            Text text = qol.GetComponent<Text>();
            if (keysWithLongNames.Contains(i.Key))
            {
                Debug.Log("Scrivo sotto " + i.Value + " il tasto " + i.Key);
                text.text = i.Key.ToString().Substring(5).ToUpper();
            }
            else
            {
                text.text = i.Key.ToString().ToUpper();
            }
        }
    }
}