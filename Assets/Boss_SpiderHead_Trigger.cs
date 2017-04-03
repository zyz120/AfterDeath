using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiderHead_Trigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == Tags.ceiling)
        {
            transform.parent.GetComponent<Boss_SpiderHead>().verticalSpeed *= -1;
        }
        else if (collision.tag == Tags.wall)
        {
            transform.parent.GetComponent<Boss_SpiderHead>().moveSpeed *= -1;
        }
        else if (collision.tag == Tags.ground)
        {
            transform.parent.GetComponent<Boss_SpiderHead>().verticalSpeed = 10f;
        }
    }


}
