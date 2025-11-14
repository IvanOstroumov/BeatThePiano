using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume")]
    public Slider volumeSlider;
    
    [Header("Color")]
    public Slider colorSlider;
    public GameObject colorDisplay;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        colorSlider.value = PlayerPrefs.GetFloat("color");
        colorSlider.onValueChanged.AddListener(SetColor);
        spriteRenderer = colorDisplay.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }
    
    void SetVolume(float v)
    {
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("volume", v);
    }

    void SetColor(float v)
    {
        Debug.Log(v);
        spriteRenderer.color = Color.HSVToRGB(v, 1, 1);
        PlayerPrefs.SetFloat("color", v);
    }
}
