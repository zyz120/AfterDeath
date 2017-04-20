﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public int type;
    public GameObject web_ground;

    public float maxScale;

    public float stayTime;

    public Vector3 velocity;

    private float timer;
    private float updateTimer;
    private bool fadeOUt;
    private bool growing;
    private SpriteRenderer render;

    void Start()
    {
        timer = updateTimer = 0;
        fadeOUt = false;
        render = GetComponent<SpriteRenderer>();
        if(type == 2)
        {
            growing = true;
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else
        {
            growing = false;
        }
    }

    private void Update()
    {
        if (type == 1)
            transform.position -= velocity.normalized * Time.deltaTime * 12f;

        if (growing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxScale, maxScale, maxScale), 2.5f * Time.deltaTime);
            if(maxScale - transform.localScale.x <= 0.01f)
            {
                growing = false;
            }
        }

        if(fadeOUt)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = c.a - 0.55f * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = c;
            if(c.a <= 0.01f)
                Destroy(gameObject);
        }
        updateTimer += Time.deltaTime;
        if (updateTimer >= 0.1f)
            updateTimer = 0;
        timer += Time.deltaTime;
        if (timer >= stayTime)
            fadeOUt = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.player)
        {
            if (type == 1)
            {
                collision.gameObject.GetComponent<PlayerController>().Hurt(2, transform.position, new Vector3(0, 0, 0), 0.8f);
                GameManager.Instance.GetBuff(Buff.BuffType.spiderWeb, 4.0f);
                Destroy(gameObject);
            }
        }
        else if (collision.tag == Tags.ground && type == 1)
        {
            Destroy(gameObject);
            GameObject go = Instantiate(web_ground, transform.position - new Vector3(0, 0.9f + Random.Range(-0.2f, 0.2f), 0), Quaternion.identity) as GameObject;
            go.GetComponent<SpiderWeb>().maxScale = Random.Range(1.2f, 1.8f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (updateTimer < 0.09f)
            return;

        if (collision.tag == Tags.player)
        {
            collision.gameObject.GetComponent<PlayerController>().Hurt(0, transform.position, new Vector3(0, 0, 0), 0.1f);
            GameManager.Instance.GetBuff(Buff.BuffType.spiderWebStop, 0.5f);
        }
    }



}   
