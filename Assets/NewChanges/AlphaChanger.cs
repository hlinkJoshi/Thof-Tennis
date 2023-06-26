using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChanger : MonoBehaviour
{
    // refer to Object which dot is connected
    public GameObject interactableObj;
    public bool isFrame = false;

    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public float changeSpeed = 1f;

    private SpriteRenderer spriteRenderer;
    private bool increasing = true;

    float alpha = 0;
    Color color = new Color(255, 255, 255, 1);

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Calculate the new alpha value
        alpha = spriteRenderer.color.a;

        if (increasing)
        {
            alpha += changeSpeed * Time.deltaTime;
            if (alpha >= maxAlpha)
            {
                alpha = maxAlpha;
                increasing = false;
            }
        }
        else
        {
            alpha -= changeSpeed * Time.deltaTime;
            if (alpha <= minAlpha)
            {
                alpha = minAlpha;
                increasing = true;
            }
        }

        // Update the sprite's alpha value
        //color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
