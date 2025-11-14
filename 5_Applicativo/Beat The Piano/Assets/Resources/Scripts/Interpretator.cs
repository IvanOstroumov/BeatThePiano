using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace BeatThePiano
{
    public class Interpretator : MonoBehaviour
    {
        //Dizionario che associa tasti di tastiera con note
        public Dictionary<KeyCode, string> notaKey = new Dictionary<KeyCode, string>();

        //Dizionario che associa nota e coordinata x
        public Dictionary<string, float> notaX = new Dictionary<string, float>
        {
            {"C", -2.4f}, {"D", -1.57f}, {"E", -0.56f}, {"F", 0f}, {"G", 0.85f}, {"A", 1.68f}, {"B", 2.5f},
            {"CSharp", -2f}, {"DSharp", -1f}, {"FSharp", 0.35f}, {"GSharp", 1.23f}, {"ASharp", 2.1f},
            {"2C", 3.18f}, {"2D", 4f}, {"2E", 4.8f}, {"2F", 5.6f}, {"2G", 6.41f}, {"2A", 7.23f}, {"2B", 8f},
            {"2CSharp", 3.55f}, {"2DSharp", 4.5f}, {"2FSharp", 5.9f}, {"2GSharp", 6.8f}, {"2ASharp", 7.73f}
        };


        //Lista di gameObject che creo
        private List<GameObject> notas = new List<GameObject>();
        public static bool isDone = false;

        void Start()
        {
            //Aggiorna i tasti a quelli salvati in memoria
            KeyRemapper.aggiornaTasti();


            //Creazione di texture per i gameObject
            Texture2D tex2 = new Texture2D(5, 5);
            //Assegnazione del colore scelto dall'utente
            Color blockColor = Color.HSVToRGB(PlayerPrefs.GetFloat("color"), 1, 1);
            tex2.SetPixel(0, 0, Color.HSVToRGB(PlayerPrefs.GetFloat("color"), 1, 1));
            tex2.Apply();

            Debug.Log("Start");

            //Caricamento di tutte le musiche
            Music firstMusic = new Music("First-Easy", Difficulty.Facile, 100,
                "./Assets/Resources/Sounds/midi/first-easy.mid");
            Music secondMusic = new Music("Second-Easy", Difficulty.Facile, 90,
                "./Assets/Resources/Sounds/midi/second-easy.mid");
            Music thirdMusic = new Music("First-Medium", Difficulty.Medio, 100,
                "./Assets/Resources/Sounds/midi/first-medium.mid");
            Music fourthMusic = new Music("Second-Medium", Difficulty.Medio, 100,
                "./Assets/Resources/Sounds/midi/second-medium.mid");
            Music fifthMusic = new Music("First-Hard", Difficulty.Difficile, 120,
                "./Assets/Resources/Sounds/midi/first-hard.mid");
            Music sixthMusic = new Music("Second-Hard", Difficulty.Difficile, 120,
                "./Assets/Resources/Sounds/midi/second-hard.mid");
            Music seventhMusic = new Music("First-Impossible", Difficulty.Difficilissimo, 180,
                "./Assets/Resources/Sounds/midi/first-impossible.mid");

            //Scelta della musica da giocare
            Music selected = firstMusic;
            //Loop che istanzia tutti i gameObject
            foreach (Nota nota in selected.Notes)
            {
                //Creazione GameObject
                GameObject blocco = new GameObject("Blocco");
                //Aggiunta componente spriteRenderer in modo che si può dargli una texture
                SpriteRenderer bloccoSpriteRenderer = blocco.AddComponent<SpriteRenderer>();
                //Creazione sprite assegnadoli la texture creata prima e lo colora nel colore assegnato nell PlayerPrefs
                bloccoSpriteRenderer.sprite = Sprite.Create(tex2, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
                bloccoSpriteRenderer.color = blockColor;
                //Aggiungo collider al blocco in modo che posso cancellarlo quando tocca il collider della deathZone
                BoxCollider bloccoBoxCollider = blocco.AddComponent<BoxCollider>();

                //Controllo in che ottava È la nota e selezione dove istanziarla (x,y)
                if (nota.Octave == 4)
                {
                    blocco.transform.position =
                        new Vector3(notaX[nota.Note] - 2.83f, nota.SpawnTime + nota.Duration, 0);
                }
                else if (nota.Octave == 5)
                {
                    blocco.transform.position =
                        new Vector3(notaX["2" + nota.Note] - 2.83f, nota.SpawnTime + nota.Duration, 0);
                }

                //Controllo se la nota è un diesis (Se si, deve avere una larghezza minore) e assegnazione grandezza alla nota
                if (nota.Note.Contains("Sharp"))
                {
                    blocco.transform.localScale = new Vector3(38, (nota.Duration * selected.Tempo) - 10, 1);
                }
                else
                {
                    blocco.transform.localScale = new Vector3(63, (nota.Duration * selected.Tempo) - 10, 1);
                }


                notas.Add(blocco);


                Debug.Log(nota.ToString());
            }
        }

        //IEnumerator -> (In questo caso) permette di stoppare un itarazione in modo che il resto del codice continua a funzionare e non fa priam tutto il while
        IEnumerator showResults()
        {
            //Trova l'oggetto che rattiene dentro di sè tutti i risultati e l'ho centra sullo schermo
            GameObject results = GameObject.Find("Results");
            bool isPianoDeleted = false;
            while (results.transform.position.y < -1.41)
            {
                float oldX = results.transform.position.x;
                float oldY = results.transform.position.y + 0.0005f;
                results.transform.position = new Vector3(oldX, oldY, -4);
                if (isPianoDeleted == false & results.transform.position.y > -6.38)
                {
                    isPianoDeleted = true;
                    GameObject piano = GameObject.Find("Piano");
                    Destroy(piano);
                }
            }

            yield return null;
        }

        void Update()
        {
            //Controlla se è finito il livello
            if (notas.Count == 0)
            {
                isDone = true;
                //Coroutine -> metodo implementato da Unity che permette di fare più azioni in parallelo
                StartCoroutine(showResults());
            }
            else
            {
                //Sposta  blocchi in giù ogni frame
                foreach (GameObject nota in notas)
                {
                    float oldPosY = nota.transform.position.y - 0.003f;
                    float oldPosX = nota.transform.position.x;
                    float oldPosZ = nota.transform.position.z;
                    nota.transform.position = new Vector3(oldPosX, oldPosY, oldPosZ);
                    if (nota.transform.position.y < -6.65)
                    {
                        notas.Remove(nota);
                        Destroy(nota);
                    }
                }
            }
        }
    }
}