using UnityEngine;
using System.Collections;

public class SmallSpiderTrigger : MonoBehaviour
{

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == Tags.player)
        {
            collision.transform.GetComponent<PlayerController>().Hurt(12, transform.position, CommonData.Instance._normalRepelForce, 0.8f);
        }
        if(collision.tag == Tags.playerBullet)
        {
            transform.parent.GetComponent<SmallSpiderAI>().TakeDamage(PlayerData.Instance._damage);
            collision.GetComponent<Player_Bullet_Demo>().ShowParticleEffect();
        }
    }

}
