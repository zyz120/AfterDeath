using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTemp : MonoBehaviour
{
    public float fadeSpeed;

    public bool isFading;

    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isFading = false;
    }

    void Update ()
    {
		if(isFading)
        {
            render.color = Color.Lerp(render.color, new Color(1.0f, 1.0f, 1.0f, 1.0f), fadeSpeed * Time.deltaTime);
            if(render.color.a >= 0.99f)
            {
                render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                isFading = false;
            }
        }
	}
}
