using System.Collections;
using UnityEngine;

namespace BeatThePiano
{
    //Classe per diminuire la quantità di codice e non riistanziare ogni volta tutti i livelli
    public class Manager
    {
        //Istanziazione dei 7 livelli
        public static Music firstMusic = new Music("First-Easy", Difficulty.Facile, 100,
            "./Assets/Resources/Sounds/midi/first-easy.mid", 1);

        public static Music secondMusic = new Music("Second-Easy", Difficulty.Facile, 90,
            "./Assets/Resources/Sounds/midi/second-easy.mid", 2);

        public static Music thirdMusic = new Music("First-Medium", Difficulty.Medio, 100,
            "./Assets/Resources/Sounds/midi/first-medium.mid", 3);

        public static Music fourthMusic = new Music("Second-Medium", Difficulty.Medio, 100,
            "./Assets/Resources/Sounds/midi/second-medium.mid", 4);

        public static Music fifthMusic = new Music("First-Hard", Difficulty.Difficile, 120,
            "./Assets/Resources/Sounds/midi/first-hard.mid", 5);

        public static Music sixthMusic = new Music("Second-Hard", Difficulty.Difficile, 120,
            "./Assets/Resources/Sounds/midi/second-hard.mid", 6);

        public static Music seventhMusic = new Music("First-Impossible", Difficulty.Difficilissimo, 180,
            "./Assets/Resources/Sounds/midi/first-impossible.mid", 7);

        public ArrayList levels;

        public Manager()
        {
            levels = new ArrayList();
            levels.Add(firstMusic);
            levels.Add(secondMusic);
            levels.Add(thirdMusic);
            levels.Add(fourthMusic);
            levels.Add(fifthMusic);
            levels.Add(sixthMusic);
            levels.Add(seventhMusic);
        }
        //Funzione che ti ritorna la musica cercandola usando l'id passato
        public Music getMusicById(int id)
        {
            foreach (Music music in levels)
            {
                Debug.Log("Confronto " + id + " con " + music.Id);
                if (music.Id == id)
                {
                    Debug.Log("TROVATO UGUALGIANZA " + music.Name);
                    return music;
                }
            }

            return firstMusic;
        }
    }
}