//Intero script generato da chatGPT
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspect : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            // Letterbox (black bars top/bottom)
            cam.rect = new Rect(
                0f,
                (1f - scaleHeight) / 2f,
                1f,
                scaleHeight
            );
        }
        else
        {
            // Pillarbox (black bars left/right)
            float scaleWidth = 1f / scaleHeight;
            cam.rect = new Rect(
                (1f - scaleWidth) / 2f,
                0f,
                scaleWidth,
                1f
            );
        }
    }
}