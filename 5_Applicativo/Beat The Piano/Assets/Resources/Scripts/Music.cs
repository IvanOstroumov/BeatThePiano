using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BeatThePiano
{
    public class Music
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Difficulty _difficulty;

        public Difficulty Difficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }


        private int _tempo;

        public int Tempo
        {
            get { return _tempo; }
            set { _tempo = value; }
        }

        private List<Nota> _notes;

        public List<Nota> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public void addNote(Nota nota)
        {
            if (nota != null)
            {
                Notes.Add(nota);
            }
        }

        public Music(string name, Difficulty difficulty, int tempo, string path, int id)
        {
            _name = name;
            _difficulty = difficulty;
            _tempo = tempo;
            _path = path;
            _notes = MidiConverter.ConvertMidiToNota(path);
            _id = id;
        }
        //Funzione che calcola il punteggio che hai fatto controllando che la nota, la ottava, e i tempi siano giusti (con un margine di errore). Alla fine lo ritorna come float
        public float calcolaPunteggio(List<Nota> playedNotes, float timingOffset = 0.45f)
        {
            if (playedNotes == null || playedNotes.Count == 0)
                return 0f;

            int correct = 0;

            foreach (Nota original in _notes)
            {
                foreach (Nota played in playedNotes)
                {
                    bool noteMatches = original.Note == played.Note;
                    bool octaveMatches = original.Octave == played.Octave;
                    bool timingMatches, durationMatches;
                    if (original.SpawnTime > played.SpawnTime)
                    {
                        timingMatches = original.SpawnTime - played.SpawnTime <= timingOffset;
                    }
                    else
                    {
                        timingMatches = played.SpawnTime - original.SpawnTime <= timingOffset;
                    }

                    if (original.Duration > played.Duration)
                    {
                        durationMatches = original.Duration - played.Duration <= timingOffset;
                    }
                    else
                    {
                        durationMatches = played.Duration - original.Duration <= timingOffset;
                    }

                    if (noteMatches && durationMatches && timingMatches && octaveMatches)
                    {
                        correct++;
                        break;
                    }
                }
            }

            return (float)correct / this.Notes.Count;
        }
    }
}