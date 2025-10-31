using System.Collections.Generic;
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
       
       private List<Nota>  _notes;
       public List<Nota> Notes
       {
           get { return _notes; }
           set { _notes = value; }
       }

       public void addNote(Nota nota)
       {
           if (nota != null)
           { 
               Notes.Add(nota);
           }
       }

       public Music(string name, Difficulty difficulty, int tempo)
       {
           _name = name;
           _difficulty = difficulty;
           _tempo = tempo;
           _notes = new List<Nota>();
       }
       
       public float calcolaPunteggio(List<Nota> playedNotes, float timingOffset = 0.2f)
       {
           if (playedNotes == null || playedNotes.Count == 0)
               return 0f;

           int correct = 0;
           
           foreach (Nota original in _notes)
           {
               foreach (Nota played in playedNotes)
               {
                   bool noteMatches = original.Note == played.Note;
                   bool timingMatches = Mathf.Abs(original.SpawnTime - played.SpawnTime) <= timingOffset;
                   
                   bool durationMatches = Mathf.Abs(original.Duration - played.Duration) <= timingOffset;

                   if (noteMatches && timingMatches && durationMatches)
                   {
                       correct++;
                       break; 
                   }
               }
           }

           return (float)correct / _notes.Count;
       }

   }
}

