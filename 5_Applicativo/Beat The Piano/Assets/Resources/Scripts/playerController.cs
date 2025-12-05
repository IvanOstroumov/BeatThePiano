using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeatThePiano
{
    public class playerController : MonoBehaviour
    {
        //Dizionario che contiene in se le note che l'utente sta tenendo premute
        private Dictionary<KeyCode, float> activeNotes = new Dictionary<KeyCode, float>();
        //Lista delle note suonate
        public List<Nota> playedNotes = new List<Nota>();
        public Music selected;

        void Start()
        {
            Manager manager = new Manager();
            selected = manager.getMusicById(PlayerPrefs.GetInt("SelectedLevel"));
        }

        void Update()
        {
            //Se il livello non È finito
            if (!Interpretator.isDone)
            {
                
                foreach (var entry in KeyRemapper.notaKey)
                {
                    KeyCode key = entry.Key;
                    string noteName = entry.Value;

                    //Controlla se il tasto è schiacciato, se si lo inserisce nel dizionario
                    if (Input.GetKeyDown(key))
                    {
                        activeNotes[key] = Time.time;
                        GameObject tasto = GameObject.Find(noteName);
                        tasto.transform.localScale = tasto.transform.localScale * 1.2f;
                        GameObject suono =  GameObject.Find(noteName + "_Sound");
                        AudioSource aud = suono.GetComponent<AudioSource>();
                        aud.PlayOneShot(aud.clip);
                        
                    }

                    //Controlla se l'utente a smesso di premere un tasto, allora istanzia una nota e la salva nella lista delle note suonate
                    if (Input.GetKeyUp(key))
                    {
                        float start = activeNotes[key];
                        float duration = Time.time - start;

                        int octave = GetOctave(noteName);
                        string cleanNote = RemovePrefix(noteName);

                        Nota nota = new Nota(start - 1.8f, duration * 2, cleanNote, octave);
                        playedNotes.Add(nota);

                        GameObject tasto = GameObject.Find(noteName);
                        tasto.transform.localScale = tasto.transform.localScale / 1.2f;

                        Debug.Log(nota.ToString());
                    }
                }
            }
            else
            {
                //Salva il punteggio se È il migliore subito
                float punteggio = selected.calcolaPunteggio(GetPlayedNotes());
                if (PlayerPrefs.GetFloat("Punteggio_" + selected.Name) < punteggio)
                {
                    PlayerPrefs.SetFloat("Punteggio_" + selected.Name, punteggio);
                }

                //Converte il punteggio in stelle
                GameObject star1 = GameObject.Find("FullStar_1");
                GameObject star2 = GameObject.Find("FullStar_2");
                GameObject star3 = GameObject.Find("FullStar_3");
                if (punteggio <= 0.33 && punteggio > 0.01)
                {
                    star2.GetComponent<SpriteRenderer>().enabled = false;
                    star3.GetComponent<SpriteRenderer>().enabled = false;
                }
                else if (punteggio > 0.33 && punteggio <= 0.66)
                {
                    star3.GetComponent<SpriteRenderer>().enabled = false;
                }
                else if (punteggio > 0.66 && punteggio <= 1)
                {
                }
                else
                {
                    star1.GetComponent<SpriteRenderer>().enabled = false;
                    star2.GetComponent<SpriteRenderer>().enabled = false;
                    star3.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        //Ritorna in quale ottava è la nota
        int GetOctave(string note)
        {
            if (note.StartsWith("2"))
                return 5;
            return 4;
        }

        //Converte la scrittura del nome della nota in quella normale
        string RemovePrefix(string note)
        {
            if (note.StartsWith("2"))
                return note.Substring(1);
            return note;
        }

        public List<Nota> GetPlayedNotes()
        {
            return playedNotes;
        }
    }
}