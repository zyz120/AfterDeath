using UnityEngine;
using System.Collections;

public class Boss_Bullet_Demo : MonoBehaviour
{

    public void Shoot(Vector2 vel)
    {
        GetComponent<Rigidbody2D>().velocity = vel;
        Destroy(gameObject, 3f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.player)
        {
			collision.GetComponent<PlayerController>().Hurt(10, transform.position,CommonData.Instance._normalRepelForce, 0.5f);
            Destroy(gameObject);
        }
    }
}
