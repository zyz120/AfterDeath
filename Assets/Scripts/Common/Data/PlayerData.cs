using UnityEngine;
using System.Collections;

public class PlayerData
{
    private static PlayerData _instance;
    public static PlayerData Instance
    { get
        {
            if (_instance == null)
                _instance = new PlayerData();
            return _instance;
        }
    }

    public float _maxHealth;//The max health of hero
    public float _damage;
    public float _defence;
    public float _moveSpeed;
    public float _jumpForce;
    public float _repulseRate;
    public float _bulletSpeed;
    public float _overHeatMax;
	public float _fireRate;
	public float _collectionSpeed;//The increment speed of the collection per frame
    public float _invicibleTimeRate;

    public float _maxHealth_origin { get; private set; }
    public float _health_origin { get; private set; }
    public float _damage_origin { get; private set; }
    public float _defence_origin { get; private set; }
    public float _moveSpeed_origin { get; private set; }
    public float _jumpForce_origin { get; private set; }
    public float _repulseRate_origin { get; private set; }
    public float _bulletSpeed_origin { get; private set; }
    public float _overHeatMax_origin { get; private set; }
    public float _fireRate_origin { get; private set; }
	public float _collectionSpeed_origin{ get; private set; }
    public float _invicibleTimeRate_origin { get; private set; }

    public PlayerData()
    {
		ResetData ();
    }

	public void ResetData()
	{
		_maxHealth = _maxHealth_origin = 100f;
		_damage = _damage_origin = 10f;
		_defence = _defence_origin = 1f;
		_moveSpeed = _moveSpeed_origin = 0.12f;
		_jumpForce = _jumpForce_origin = 750f;
		_repulseRate = _repulseRate_origin = 1f;
		_bulletSpeed = _bulletSpeed_origin = 15f;
		_overHeatMax = _overHeatMax_origin = 100f;
		_fireRate = _fireRate_origin = 5f;
		_collectionSpeed = _collectionSpeed_origin = 1f;
        _invicibleTimeRate = _invicibleTimeRate_origin = 1f;
	}
}