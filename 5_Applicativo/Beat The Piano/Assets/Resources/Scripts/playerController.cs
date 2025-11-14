using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeatThePiano
{
    public class playerController : MonoBehaviour
    {
        private Dictionary<KeyCode, float> activeNotes = new Dictionary<KeyCode, float>();
        public List<Nota> playedNotes = new List<Nota>();

        void Update()
        {
            if (!Interpretator.isDone)
            {
                foreach (var entry in KeyRemapper.notaKey)
                {
                    KeyCode key = entry.Key;
                    string noteName = entry.Value;

                    if (Input.GetKeyDown(key))
                    {
                        activeNotes[key] = Time.time;
                        GameObject tasto = GameObject.Find(noteName);
                        tasto.transform.localScale = tasto.transform.localScale * 1.2f;
                    }

                    if (Input.GetKeyUp(key))
                    {
                        float start = activeNotes[key];
                        float duration = Time.time - start;

                        int octave = GetOctave(noteName);
                        string cleanNote = RemovePrefix(noteName);

                        Nota nota = new Nota(start, duration, cleanNote, octave);
                        playedNotes.Add(nota);
                        
                        GameObject tasto = GameObject.Find(noteName);
                        tasto.transform.localScale = tasto.transform.localScale / 1.2f;

                        Debug.Log(nota.ToString());
                    }
                }
            }
            else
            {
                //Caricamento di tutte le musiche
                Music firstMusic = new Music("First-Easy", Difficulty.Facile, 100, "./Assets/Resources/Sounds/midi/first-easy.mid");
                Music secondMusic = new Music("Second-Easy", Difficulty.Facile, 90, "./Assets/Resources/Sounds/midi/second-easy.mid");
                Music thirdMusic = new Music("First-Medium", Difficulty.Medio, 100, "./Assets/Resources/Sounds/midi/first-medium.mid");
                Music fourthMusic = new Music("Second-Medium", Difficulty.Medio, 100, "./Assets/Resources/Sounds/midi/second-medium.mid");
                Music fifthMusic = new Music("First-Hard", Difficulty.Difficile, 120, "./Assets/Resources/Sounds/midi/first-hard.mid");
                Music sixthMusic = new Music("Second-Hard", Difficulty.Difficile, 120, "./Assets/Resources/Sounds/midi/second-hard.mid");
                Music seventhMusic = new Music("First-Impossible", Difficulty.Difficilissimo, 180, "./Assets/Resources/Sounds/midi/first-impossible.mid");
                Debug.Log(firstMusic.calcolaPunteggio(GetPlayedNotes()));
            }
        }

        int GetOctave(string note)
        {
            if (note.StartsWith("2"))
                return 5;
            return 4;
        }

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