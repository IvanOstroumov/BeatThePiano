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
           _notes.Add(nota);
       }

   } 
}

