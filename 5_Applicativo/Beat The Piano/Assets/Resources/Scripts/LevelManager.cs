using System.Collections.Generic;
using UnityEngine;

namespace BeatThePiano
{
    public class LevelManager : MonoBehaviour
    {
        //Dizionario che associa un livello e il tuo massimo punteggio in quello
        public Dictionary<Music, float> risultati = new Dictionary<Music, float>();

        void Start()
        {
            Manager manager = new Manager();
            //Popola il dizionario
            foreach (Music level in manager.levels)
            {
                float risultato = PlayerPrefs.GetFloat("Punteggio_" + level.Name);
                risultati.Add(level, risultato);
            }

            //Ciclo che nasconde i lucchetti o i bottoni play a dipendenza di quali livelli hai gia giocato (Se il punteggio massimo è più di 0, sblocca il prossimo livello)
            foreach (KeyValuePair<Music, float> pair in risultati)
            {
                if (pair.Key.Id != 7)
                {
                    int id = pair.Key.Id + 1;
                    if (pair.Value != 0)
                    {
                        GameObject lockk = GameObject.Find("lock_" + id);
                        Debug.Log("Sto cancellando lock_" + id);
                        lockk.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        GameObject play = GameObject.Find("Play_" + id);
                        Debug.Log("Sto cancellando Play_" + id);
                        play.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                //Mostra il punteggio massimo sottoforma di stelle /3
                float punteggio = PlayerPrefs.GetFloat("Punteggio_" + pair.Key.Name);
                GameObject star1 = GameObject.Find("FullStar_" + pair.Key.Id + "_1");
                GameObject star2 = GameObject.Find("FullStar_" + pair.Key.Id + "_2");
                GameObject star3 = GameObject.Find("FullStar_" + pair.Key.Id + "_3");
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
    }
}