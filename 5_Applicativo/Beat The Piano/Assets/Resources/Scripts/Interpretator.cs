using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatThePiano
{
    public class Interpretator : MonoBehaviour
    {
        //Dizionario che associa tasti di tastiera con note
        public Dictionary<KeyCode, string> notaKey = new Dictionary<KeyCode, string>();

        //Dizionario che associa nota e coordinata x
        public Dictionary<string, float> notaX = new Dictionary<string, float>
        {
            { "C", -2.4f }, { "D", -1.57f }, { "E", -0.56f }, { "F", 0f }, { "G", 0.85f }, { "A", 1.68f },
            { "B", 2.5f },
            { "CSharp", -2f }, { "DSharp", -1f }, { "FSharp", 0.35f }, { "GSharp", 1.23f }, { "ASharp", 2.1f },
            { "2C", 3.18f }, { "2D", 4f }, { "2E", 4.8f }, { "2F", 5.6f }, { "2G", 6.41f }, { "2A", 7.23f },
            { "2B", 8f },
            { "2CSharp", 3.55f }, { "2DSharp", 4.5f }, { "2FSharp", 5.9f }, { "2GSharp", 6.8f }, { "2ASharp", 7.73f }
        };


        //Lista di gameObject che creo
        private List<GameObject> notas = new List<GameObject>();

        //Variabile statica che specfica allo script playerController.cs quando calcolare i risultati
        public static bool isDone = false;

        void Start()
        {
            Manager manager = new Manager();
            ArrayList levels = manager.levels;
            
            //Azzeramento di una variabile statica perché non si azzerano da sole al caricamento della scena unity
            isDone = false;
            
            //Aggiorna i tasti a quelli salvati in memoria
            KeyRemapper.aggiornaTasti();


            //Creazione di texture per i gameObject
            Texture2D tex2 = new Texture2D(5, 5);
            //Assegnazione del colore scelto dall'utente
            Color blockColor = Color.HSVToRGB(PlayerPrefs.GetFloat("color"), 1, 1);
            
            //Creazione di un bordo nero
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    tex2.SetPixel(x, y, Color.black);
                }
            }

            //Riempimento del blocco con il colore
            for (int x = 1; x <= 3; x++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    tex2.SetPixel(x, y, blockColor);
                }
            }

            //Questo parametro permette di rendere la texture di alta qualità pure se viene ingrandità (Applicata a GO grandi)
            tex2.filterMode = FilterMode.Point;
            tex2.Apply();

            Debug.Log("Start");

            //Scelta della musica da giocare
            int id = PlayerPrefs.GetInt("SelectedLevel");
            Debug.Log("ID: " + id);
            Music selected = manager.getMusicById(id);
            
            //Loop che istanzia tutti i gameObject
            foreach (Nota nota in selected.Notes)
            {
                //Creazione GameObject
                GameObject blocco = new GameObject("Blocco");
                //Aggiunta componente spriteRenderer in modo che si può dargli una texture
                SpriteRenderer bloccoSpriteRenderer = blocco.AddComponent<SpriteRenderer>();
                //Creazione sprite assegnadoli la texture creata prima e lo colora nel colore assegnato nell PlayerPrefs
                bloccoSpriteRenderer.sprite = Sprite.Create(tex2, new Rect(0, 0, 5, 5), new Vector2(0.5f, 0.5f));
                bloccoSpriteRenderer.color = Color.white;
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
                    blocco.transform.localScale = new Vector3(38 / 5, ((nota.Duration * selected.Tempo) - 10) / 5, 1);
                }
                else
                {
                    blocco.transform.localScale = new Vector3(63 / 5, ((nota.Duration * selected.Tempo) - 10) / 5, 1);
                }
                
                notas.Add(blocco);
                Debug.Log(nota.ToString());
            }
        }

        //IEnumerator -> (In questo caso) permette di stoppare un itarazione in modo che il resto del codice continua a funzionare e non fa priam tutto il while
        IEnumerator showResults()
        {
            //Trova l'oggetto che rattiene dentro di sè tutti i risultati e l'ho centra sullo schermo cancellando il piano per maggiore pulizia
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

                yield return null;
            }
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
                    float oldPosY = nota.transform.position.y - (0.003f / 1.28f);
                    float oldPosX = nota.transform.position.x;
                    float oldPosZ = nota.transform.position.z;
                    nota.transform.position = new Vector3(oldPosX, oldPosY, oldPosZ);
                }

                //Uccide un blocco se è non lo si vede più sullo schermo
                for (int i = notas.Count - 1; i >= 0; i--)
                {
                    if (notas[i].transform.position.y < -6.65)
                    {
                        Destroy(notas[i]);
                        notas.RemoveAt(i);
                    }
                }
            }
        }
    }
}