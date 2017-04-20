using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider : Boss {

    public enum State { move, egg, change, spin };
    public State currentState;

    public GameObject eggPrefab;
    public GameObject spinPrefab;
    public GameObject spiderHeadPrefab;
    public GameObject smallSpiderPrefab;

    public UnityArmatureComponent armature;
    public UnityEngine.Transform player;
    public UnityEngine.Transform spinPos;
    public float rotateSpeed;
    public float moveSpeed;

    private float eggTimer;
    private float spinTimer;

    private bool changed;


    void Start () 
    {
        changed = false;
        player = GameObject.Find("Player").transform;
        currentState = State.move;
        eggTimer = spinTimer = 0.0f;
        BornEgg();
	}
	
	void Update ()
    {
        LookAtPlayer();
        FollowPlayer();
        CheckMovePosition();
        HpListener();

        eggTimer += Time.deltaTime;
        spinTimer += Time.deltaTime;

        Attack();

        if(Input.GetMouseButtonDown(1))
        {
            BornEgg();
        }

    }

    void LookAtPlayer()
    {
        float targetAngle = Mathf.Clamp(Vector3.Angle(new Vector3(0, -1, 0), player.position - transform.position), 0f, 12f) * (transform.position.x > player.position.x ? -1 : 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotateSpeed * Time.deltaTime);
    }

    void FollowPlayer()
    {
        if (currentState != State.move)
            return;

        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
    }    

    void CheckMovePosition()
    {
        if(currentState == State.move)
        {
            if(transform.position.x > player.position.x)
            {
                if (armature.animation.lastAnimationName != "move_right")
                    armature.animation.Play("move_right");
            }
            else
            {
                if (armature.animation.lastAnimationName != "move_left")
                    armature.animation.Play("move_left");
            }
        }
    }

    void BornEgg()
    {
        if (currentState == State.change)
            return;

        currentState = State.egg;
        armature.animation.Play("egg");

        Invoke("BornEggInvoke", 1.5f);
    }

    void BornEggInvoke()
    {
        if (currentState == State.change)
            return;
        currentState = State.move;

        GameObject egg = Instantiate(eggPrefab, new Vector2(Random.Range(-7.7f, 7.7f), Random.Range(3.0f, 5.0f)), Quaternion.identity) as GameObject;
        float scale = Random.Range(0.4f, 0.7f);
        egg.GetComponentInChildren<SpiderEgg>().maxScale = scale;
    }

    void Spin()
    {
        if (currentState == State.change)
            return;

        currentState = State.spin;
        armature.animation.Play("spin_1");

        Invoke("SpinInvoke", 0.4f);

    }

    void SpinInvoke()
    {
        if (currentState == State.change)
            return;
        currentState = State.move;

        GameObject spin = Instantiate(spinPrefab, spinPos.position, Quaternion.identity) as GameObject;

        spin.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (transform.position.x > player.position.x ? -1 : 1) * Vector3.Angle(new Vector3(0, -1, 0), player.position - transform.position)));

        spin.GetComponent<SpiderWeb>().velocity = spinPos.position - new Vector3(player.transform.position.x, Mathf.Clamp(player.transform.position.y, -5f, -2.5f), player.transform.position.z);
    }

    void Attack()
    {
        if(eggTimer >= Random.Range(8.0f, 12.0f))
        {
            if (currentState == State.spin)
            {
                eggTimer -= 2.0f;
                return;
            }
            BornEgg();
            eggTimer = 0;
        }
        if(spinTimer >= Random.Range(5.0f, 8.0f))
        {
            if (currentState == State.egg)
            {
                spinTimer -= 2.0f;
                return;
            }
            Spin();
            spinTimer = 0;
        }
    }

    void HpListener()
    {
        if(health <= 100)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (changed)
                return;
            changed = true;
            currentState = State.change;
            if (armature.animation.lastAnimationName != "bellow")
            {
                armature.animation.timeScale = 0.7f;
                armature.animation.Play("bellow");

                for(int i = 0;i < 6;i++)
                {
                    GameObject web = Instantiate(spinPrefab, new Vector3(-7.5f + 3 * i, Random.Range(2.5f, 3.5f), 0), Quaternion.identity) as GameObject;
                    web.GetComponent<SpiderWeb>().velocity = new Vector3(0, 1, 0);
                }

                StartCoroutine(BreakEggs());
                StartCoroutine(ShakeScreen());
                Invoke("BornHead", 2.5f);
            }
        }
    }

    void BornHead()
    {
        for(int i = 0 ;i < 4;i++)
        {
            Instantiate(smallSpiderPrefab,  new Vector3(transform.position.x - 0.3f + i * 0.2f, transform.position.y, transform.position.z), Quaternion.identity);
        }

        Instantiate(spiderHeadPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator ShakeScreen()
    {
        for(int i =0;i < 40;i++)
        {
            float x = Random.Range(-0.2f, 0.2f);
            float y = Random.Range(-0.2f, 0.2f);

            Camera.main.transform.position = new Vector3(x, y, -10);

            yield return new WaitForSecondsRealtime(0.04f);
        }
    }

    IEnumerator BreakEggs()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag(Tags.spiderEgg);

        for(int i = 0; i < eggs.Length; i++)
        {
            eggs[i].GetComponent<SpiderEgg>().Break(3);
            yield return new WaitForSecondsRealtime(0.2f);
            Debug.Log(eggs.Length + "    " + i);
        }

    }

}
