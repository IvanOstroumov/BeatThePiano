using System.ComponentModel;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [Header("Play Music? 0=NO 1=YES")] public bool playMusic = true;

    void Start()
    {
        if (playMusic)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
        }
        else
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
        }
    }

    void Update()
    {
    }
}