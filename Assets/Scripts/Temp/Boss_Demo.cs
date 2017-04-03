using UnityEngine;
using System.Collections;

public class Boss_Demo : MonoBehaviour
{
    public Transform _fromPos;
    public Transform _toPos;
    public Transform _player;
    public float _range;
    public float _rate;
    public float _shootRate;
    public bool _bossMove = false;

    public GameObject _bulletPrefab;

    private float circleRate;
    private float timer = 0;
    private float shootTimer = 0f;
    public Transform spawnPos;

    private float health;


    void Awake()
    {
        health = 1000f;
        circleRate = _range;
        _shootRate = 0.8f;
        _bossMove = true;
    }


    void Update()
    {
        shootTimer += Time.deltaTime; // 射击计时器

         MoveSphere();    // 弧线移动
        LookAtPlayer();  // 朝向角色

        if (shootTimer > Random.Range(_shootRate*0.5f, _shootRate*1.5f))
            Shoot();

        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShootLazer());
        }
    }

    void MoveSphere()
    {
        if (_bossMove)
        {
            timer += Time.deltaTime;

            // 弧线的中心
            Vector3 center = (_fromPos.position + _toPos.position) * 0.5f;

            // 向下移动中心，垂直于弧线
            center -= new Vector3(0, circleRate, 0);

            // 相对于中心在弧线上插值

            Vector3 riseRelCenter = _fromPos.position - center;

            Vector3 setRelCenter = _toPos.position - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, timer * _rate);

            transform.position += center;

            if ((transform.position - _toPos.position).magnitude < 0.1f)
            {
                Transform temp;
                temp = _toPos;
                _toPos = _fromPos;
                _fromPos = temp;
                timer = 0;
                circleRate = _range;
            }
        }

    }

    void LookAtPlayer()
    {
        float angle = Vector3.Angle(Vector3.right, _player.position - transform.position);
        transform.rotation = Quaternion.Euler(0, 0, transform.position.y > _player.position.y ? 360 - angle : angle);
    }

    void Shoot()
    {
        shootTimer = 0f;

        GameObject bullet = Instantiate(_bulletPrefab, spawnPos.position, Quaternion.identity) as GameObject;

        Vector2 vel = new Vector2(15f, 0);
        vel = spawnPos.rotation * vel;

        bullet.GetComponent<Boss_Bullet_Demo>().Shoot(vel);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
        if(health <= 400)
        {
            Vector3 tempPos = transform.position;
            GetComponent<SpriteRenderer>().sprite = Resources.Load("Pictures/boss_demo_2", typeof(Sprite)) as Sprite;
            _rate = 0.9f;
            _shootRate = 0.6f;
            transform.position = tempPos;
        }
        if(health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(Blink());
    }

    void ShootLazerFunction()
    {
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == Tags.player)
        {
			collision.gameObject.GetComponent<PlayerController> ().Hurt (10, transform.position, CommonData.Instance._normalRepelForce, 0.8f);
        }
        if (collision.transform.tag == Tags.playerBullet)
        {
            TakeDamage(PlayerData.Instance._damage);
        }
    }

    IEnumerator Blink()
    {
        for(int i = 0;i < 2;i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator ShootLazer()
    {
        RaycastHit2D hit;
        int count = 0;
        Vector3 direction = _player.position - spawnPos.position;
        direction.z = 0f;
        while (count <= 60)
        {
            hit = Physics2D.Raycast(spawnPos.position, direction + new Vector3(count * 0.2f, 0, 0), 10000, (1 << 8));

            if (hit.collider == null)
                break;

            count++;

            Debug.DrawLine(spawnPos.position, hit.point, Color.red);

            yield return new WaitForSeconds(0.01f);

        }
    }

}
