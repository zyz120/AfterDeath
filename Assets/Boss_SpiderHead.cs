using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpiderHead : Boss
{
    public float rotateSpeed;
    public float moveSpeed;
    public float verticalSpeed;
    public float a;

    private void Start()
    {
        moveSpeed *= Random.Range(1, 11) < 5 ? -1 : 1;
        verticalSpeed = Random.Range(-2.0f, 2.0f);
    }

    void Update()
    {
        Move();
        Rotate();

        verticalSpeed -= a * Time.deltaTime;

    }

    void Move()
    {
        transform.position += new Vector3(moveSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

}
