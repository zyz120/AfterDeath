using UnityEngine;
using System.Collections;

public class Player_Bullet_Demo : MonoBehaviour
{

    public GameObject _flarePrefab;

    void Update()
    {
        
    }

    public void Shoot(Vector2 vel)
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > GameObject.Find("horizontalFlag").transform.position.y)
        {
            vel = new Vector2(vel.x, Mathf.Abs(vel.y));
        }
        else
        {
            vel = new Vector2(vel.x, -Mathf.Abs(vel.y));
        }

        GetComponent<Rigidbody2D>().velocity = vel;
        //GetComponent<Rigidbody2D>().gravityScale = 2.0f;

        Destroy(gameObject, 3f);
    }

    public void ShowParticleEffect()
    {
        GameObject go = Instantiate(_flarePrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(go, 1f);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != Tags.player && collision.tag != Tags.untagged && collision.tag != Tags.resource)
        {
            ShowParticleEffect();
        }
    }

}
