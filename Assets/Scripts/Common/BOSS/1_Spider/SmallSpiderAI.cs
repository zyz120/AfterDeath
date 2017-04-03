using UnityEngine;
using System.Collections;

public class SmallSpiderAI : MonoBehaviour {

    float health = 50f;
    Animator anim;
    GameObject _player;

    bool isMoving;

    private float speed;

    void Awake()
    {
        anim = GetComponent<Animator>();
        isMoving = true;
        speed = Random.Range(1.0f, 4.0f);
        _player = GameObject.FindGameObjectWithTag(Tags.player);
    }


    void Update()
    {
        if(isMoving)
            transform.position += new Vector3((IsFacingRight()?speed:-speed) * Time.deltaTime, 0, 0);

        if (IsFacingRight())
        {
            transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
        }
        else
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }

        if(Vector3.Distance(_player.transform.position, transform.position) < 2.5f)
        {
            if(Random.Range(0.1f, 10.0f) < 0.5f)
                anim.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            isMoving = false;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 400));
            transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1.0f);
            return;
        }
        StartCoroutine(Blink());
    }

    public void Attack()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2((IsFacingRight()?1:-1 )* 400, 550));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>如果是向右则返回true</returns>
    public bool IsFacingRight()
    {
        if (GameObject.FindGameObjectWithTag(Tags.player).transform.position.x > transform.position.x)
            return true;
        else
            return false;
    }

    public void SetIsMovingToTrue()
    {
        isMoving = true;
    }
    public void SetIsMovingToFalse()
    {
        isMoving = false;
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 2; i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }


}
