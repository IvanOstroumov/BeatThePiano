using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume")] public Slider volumeSlider;

    [Header("Color")] public Slider colorSlider;
    public GameObject colorDisplay;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        //Imposta allo slider il valore salvato sui PlayerPrefs
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        //Aggiunge un trigger che esegue la funzione SetVolume quando il slider cambia di valore
        volumeSlider.onValueChanged.AddListener(SetVolume);

        //Imposta allo slider e al quadratino il colore salvato
        spriteRenderer = colorDisplay.GetComponent<SpriteRenderer>();
        colorSlider.value = PlayerPrefs.GetFloat("color");
        //Aggiunge un trigger che esegue la funzione SetColor quando lo slider cambia di valore
        colorSlider.onValueChanged.AddListener(SetColor);
        spriteRenderer.color = Color.HSVToRGB(colorSlider.value, 1, 1);
    }
    void SetVolume(float v)
    {
        //Imposta il volume a quello scelto e lo salva nei PlayerPrefs
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("volume", v);
    }

    void SetColor(float v)
    {
        //Imposta il colore al quadretino e lo salva nei PlayerPrefs
        spriteRenderer.color = Color.HSVToRGB(v, 1, 1);
        PlayerPrefs.SetFloat("color", v);
    }
}