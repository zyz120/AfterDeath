using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float health;
    public int attack;

    public bool isInvincible;

	// Use this for initialization
	void Start ()
    {
        isInvincible = false;	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void TakeDamage(float damage)
    {
        if (isInvincible)
            return;

        health -= damage;
        if (health <= 0)
        {
            // TODO
            Destroy(gameObject);
            GameObject.Find("Player").GetComponent<PlayerController>().Dead(2);
            return;
        }
        StartCoroutine(Blink());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == Tags.player)
        {
            collision.gameObject.GetComponent<PlayerController>().Hurt(attack, transform.position, CommonData.Instance._normalRepelForce, 0.8f);
        }
        if (collision.transform.tag == Tags.playerBullet)
        {
            TakeDamage(PlayerData.Instance._damage);
        }
    }

    IEnumerator Blink()
    {
        isInvincible = true;
        
        for (int i = 0; i < 2; i++)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().enabled = false;
                yield return new WaitForSeconds(0.05f);
                GetComponent<MeshRenderer>().enabled = true;
                yield return new WaitForSeconds(0.05f);
            }
            else if(GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.05f);
                GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
            }
        }
        isInvincible = false;
    }


}
