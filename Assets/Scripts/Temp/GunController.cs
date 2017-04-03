using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunController : MonoBehaviour {

    public GameObject _bulletPrefab;
    public Transform _spawnPos;
    public AudioClip _shootClip;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Shot.
	/// </summary>
	public void Shot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _spawnPos.position, Quaternion.identity) as GameObject;

        Vector2 vel = new Vector2(PlayerData.Instance._bulletSpeed, 0);        
        vel = GetComponentInChildren<Transform>().localRotation * vel;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < _spawnPos.position.x)
            vel = new Vector2(vel.x, -vel.y);
        /*
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < _spawnPos.position.y)
            vel = new Vector2(vel.x, Mathf.Abs(vel.y));
            */

        AudioSource.PlayClipAtPoint(_shootClip, transform.position);

        bullet.GetComponent<Player_Bullet_Demo>().Shoot(vel);
    }

	/// <summary>
	/// Rotates towards the mouse position.
	/// </summary>
	/// <param name="mousePosition">Mouse position.</param>
	public void RotateToMouse(Vector3 mousePosition)
	{
        float angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);
        angle = mousePosition.y > transform.position.y ? angle : -angle;

        if(mousePosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, -1, 1);
            angle = -angle;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

     //   angle += mousePosition.x > transform.position.x ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, 0, angle);
	}
}
