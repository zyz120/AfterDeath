using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEgg : MonoBehaviour 
{
    public GameObject smallSpider;
    public float smoothing;

    public float maxScale;

    private SpriteRenderer renderer;
    private bool isBreak;
    private bool isBorning;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

	void Start () 
    {
        isBreak = false;
        isBorning = true;

        float startScale = 0.2f;
        transform.parent.localScale = new Vector3(startScale, startScale, startScale);
	}

    void Update()
    {
        if(isBorning)
        {
            renderer.color = Color.Lerp(renderer.color, Color.white, smoothing * Time.deltaTime);
            transform.parent.localScale = Vector3.Lerp(transform.parent.localScale, Vector3.one, smoothing * Time.deltaTime);

            if (transform.parent.localScale.x >= maxScale-0.01f)
            {
                isBorning = false;
                transform.parent.localScale = new Vector3(maxScale, maxScale, maxScale);
            }


        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(isBreak)
        {
            return;
        }

        if(collider.tag == Tags.playerBullet)
        {
            Break(2);
        }
    }

    public void Break(int n)
    {
        GetComponent<Animator>().Play("SpiderEggBreak");
        GetComponent<BoxCollider2D>().enabled = false;
        isBreak = true;
        Instantiate(smallSpider, transform.position - new Vector3(0.5f, 0, 0), Quaternion.identity);
        Instantiate(smallSpider, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        if (n == 3)
            Instantiate(smallSpider, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 3.0f);
    }

}
