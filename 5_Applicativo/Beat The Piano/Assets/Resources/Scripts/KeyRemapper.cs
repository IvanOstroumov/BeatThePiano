using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyRemapper : MonoBehaviour
{
    private Text text;

    private GameObject QoL;
    //Variabili che servono per capire in che situazione ci si trova 
    private PianoKey keyBeingRemapped = null;
    private bool waitingForKey = false;

    //Dizionario che associa tasto su tastiera e nota
    public static Dictionary<KeyCode, string> notaKey = new Dictionary<KeyCode, string>
    {
        { KeyCode.Tab, "C" }, { KeyCode.Q, "D" }, { KeyCode.W, "E" }, { KeyCode.E, "F" }, { KeyCode.R, "G" },
        { KeyCode.T, "A" }, { KeyCode.Y, "B" }, { KeyCode.U, "2C" }, { KeyCode.I, "2D" }, { KeyCode.O, "2E" },
        { KeyCode.P, "2F" }, { KeyCode.Delete, "2G" }, { KeyCode.End, "2A" }, { KeyCode.PageDown, "2B" },
        { KeyCode.Alpha1, "CSharp" }, { KeyCode.Alpha2, "DSharp" }, { KeyCode.Alpha3, "FSharp" },
        { KeyCode.Alpha4, "GSharp" }, { KeyCode.Alpha5, "ASharp" },
        { KeyCode.Alpha6, "2CSharp" }, { KeyCode.Alpha7, "2DSharp" }, { KeyCode.Alpha8, "2FSharp" },
        { KeyCode.Alpha9, "2GSharp" }, { KeyCode.Alpha0, "2ASharp" }
    };

    public static Dictionary<KeyCode, string> defaultNotaKey = new Dictionary<KeyCode, String>
    {
        { KeyCode.Tab, "C" }, { KeyCode.Q, "D" }, { KeyCode.W, "E" }, { KeyCode.E, "F" }, { KeyCode.R, "G" },
        { KeyCode.T, "A" }, { KeyCode.Y, "B" }, { KeyCode.U, "2C" }, { KeyCode.I, "2D" }, { KeyCode.O, "2E" },
        { KeyCode.P, "2F" }, { KeyCode.Delete, "2G" }, { KeyCode.End, "2A" }, { KeyCode.PageDown, "2B" },
        { KeyCode.Alpha1, "CSharp" }, { KeyCode.Alpha2, "DSharp" }, { KeyCode.Alpha3, "FSharp" },
        { KeyCode.Alpha4, "GSharp" }, { KeyCode.Alpha5, "ASharp" },
        { KeyCode.Alpha6, "2CSharp" }, { KeyCode.Alpha7, "2DSharp" }, { KeyCode.Alpha8, "2FSharp" },
        { KeyCode.Alpha9, "2GSharp" }, { KeyCode.Alpha0, "2ASharp" }
    };

    //Lista di note
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

    //Funzione che cerca nei PlayerPrefs i tasti gia rimappati e gli carica sul dizionario
    public static void aggiornaTasti()
    {
        foreach (string note in notes)
        {
            if (PlayerPrefs.HasKey(note))
            {
                string keyString = PlayerPrefs.GetString(note);

                //Prova a trasformare il contenuto della PlayerPref in un tasto KeyCode, se riesce lo salva come key
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
        //Funzione che quando clicchi col mouse, crea un raggio dove l'hai creata e controlla se ha colpito qualche GO con la componente PianoKey,
        //Se si, salva quale nota si vuole rimappare e passa al prossimo blocco di codice
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
                    GameObject remappingTile = GameObject.Find("Remapping_Tile");
                    GameObject remappingText = GameObject.Find("Remapping_Text");
                    Text text = remappingText.GetComponent<Text>();
                    text.text = "Remapping:\n" + keyBeingRemapped.noteName + "\nTo\n";
                    remappingTile.transform.position = new Vector3(remappingTile.transform.position.x, -3.13f,
                        remappingTile.transform.position.z);
                    remappingText.transform.position = new Vector3(remappingText.transform.position.x, 80f,
                        remappingText.transform.position.z);
                    GameObject QoL =  GameObject.Find("QoL");
                    Graphic[] graphics = QoL.GetComponentsInChildren<Graphic>();
                    foreach (var g in graphics)
                    {
                        g.enabled = false;
                    }
                }
            }
        }

        //Funzione che assegna il tasto che hai schiacciato (non LMB) alla nota scelta prima 
        if (waitingForKey && Input.anyKeyDown)
        {
            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k) && k != KeyCode.Mouse0)
                {
                    if (!Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (notaKey.ContainsKey(k))
                        {
                            string oldNote = notaKey[k];
                            PlayerPrefs.DeleteKey(oldNote);
                        }

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
                        keyBeingRemapped.key = k;
                        Debug.Log("Remapping: " + keyBeingRemapped.noteName + " TO " + k.ToString());
                        GameObject remappingTile = GameObject.Find("Remapping_Tile");
                        GameObject remappingText = GameObject.Find("Remapping_Text");
                        Text text = remappingText.GetComponent<Text>();
                        text.text += keyBeingRemapped.key.ToString();
                        remappingTile.transform.position = new Vector3(remappingTile.transform.position.x, -8f,
                            remappingTile.transform.position.z);
                        remappingText.transform.position = new Vector3(remappingText.transform.position.x, -425f,
                            remappingText.transform.position.z);
                        PlayerPrefs.SetString(keyBeingRemapped.noteName, k.ToString());
                        PlayerPrefs.Save();

                        Debug.Log($"{keyBeingRemapped.noteName} rimappato a {k}");
                        waitingForKey = false;
                        keyBeingRemapped = null;
                        GameObject QoL =  GameObject.Find("QoL");
                        Graphic[] graphics = QoL.GetComponentsInChildren<Graphic>();
                        foreach (var g in graphics)
                        {
                            g.enabled = true;
                        }
                        ScreenKeyLoader.load();
                        break;
                    }
                    else
                    {
                        waitingForKey = false;
                        keyBeingRemapped = null;
                        GameObject remappingTile = GameObject.Find("Remapping_Tile");
                        GameObject remappingText = GameObject.Find("Remapping_Text");
                        Text text = remappingText.GetComponent<Text>();
                        remappingTile.transform.position = new Vector3(remappingTile.transform.position.x, -8f,
                            remappingTile.transform.position.z);
                        remappingText.transform.position = new Vector3(remappingText.transform.position.x, -425f,
                            remappingText.transform.position.z);
                        GameObject QoL =  GameObject.Find("QoL");
                        Graphic[] graphics = QoL.GetComponentsInChildren<Graphic>();
                        foreach (var g in graphics)
                        {
                            g.enabled = true;
                        }
                    }
                }
            }
        }
    }
}