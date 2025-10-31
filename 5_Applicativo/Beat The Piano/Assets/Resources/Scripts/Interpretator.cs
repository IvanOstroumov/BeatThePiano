using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatThePiano
{
    public class Interpretator : MonoBehaviour
    {
            public static Dictionary<string,string> notaKey = new Dictionary<string, string>();
            public static Dictionary<string, float> notaX = new Dictionary<string, float>();
            public static List<GameObject> notas = new List<GameObject>();

            void Start()
            {
                notaX.Add("A", -2.4f);
                notaX.Add("B", -1.57f);
                notaX.Add("C", -0.56f);
                notaX.Add("D", 0f);
                notaX.Add("E", 0.85f);
                notaX.Add("F", 1.68f);
                notaX.Add("G", 2.5f);


                GameObject note = new GameObject("Note");
                SpriteRenderer spriteRenderer = note.AddComponent<SpriteRenderer>();
                Texture2D tex = new Texture2D(100, 100);
                tex.SetPixel(0,0, Color.HSVToRGB(PlayerPrefs.GetFloat("color"), 1, 1));
                tex.Apply();
                spriteRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

                note.transform.position = new Vector3(-0.151f, -1.368f, -8);
                note.transform.localScale = new Vector3(45, 300, 1);
                Debug.Log("Start");

                List<Nota> firstLVL =  MidiConverter.ConvertMidiToNota("./Assets/Resources/Sounds/midi/first-hard.mid");
                foreach (Nota nota in firstLVL)
                {
                    GameObject blocco = new GameObject("Blocco");
                    SpriteRenderer bloccoSpriteRenderer = blocco.AddComponent<SpriteRenderer>();
                    Texture2D tex2 = new Texture2D(100, 100);
                    tex.SetPixel(0,0, Color.HSVToRGB(PlayerPrefs.GetFloat("color"), 1, 1));
                    tex2.Apply();
                    bloccoSpriteRenderer.sprite = Sprite.Create(tex2, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
                    
                    
                    blocco.transform.position = new Vector3(notaX[nota.Note], nota.SpawnTime + nota.Duration, -8);
                    blocco.transform.localScale = new Vector3(45, nota.Duration * 100, 1);
                    
                    
                     notas.Add(blocco);
                    
                    
                    
                    Debug.Log(nota.ToString());
                }
                
            }

            /*void Update()
            {
                foreach (GameObject nota in notas)
                {
                    nota.transform.position = 
                }
            }*/
    }
}