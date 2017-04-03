using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SceneManager:MonoBehaviour
{
	/// <summary>
	/// The gameObjects in the scene.
	/// </summary>
	public PlayerController _hero;
	public BossController _boss;

	/// <summary>
	/// The infomation of this checkpoint
	/// </summary>
	public int _checkPointId;//start from 1
	public float _animTime;//The time of the animation before fighting

	/// <summary>
	/// The positions of gameObjects.
	/// </summary>
	public Vector3 _playerPosition;
	public Vector3 _bossPostion;
	public Dictionary<Vector3,CommonData.ResourceType> _resourceList;

	void Awake()
	{
		_playerPosition = new Vector3 (-7, -4, 0);
	}

	void Start()
	{
		InitScene ();
	}

	void InitScene()
	{
		InstantiateHero ();
		InstantiateBoss ();
	}

	void AwakeFight()
	{
		_hero._controllable = true;
		_boss.Active ();
	}

	public void ShowEndding()
	{
	}

	void InstantiateHero()
	{
		_hero = GameManager.Instance._player.InstantiateHero (_playerPosition);
	}

	void InstantiateBoss()
	{
		_boss = Camera.Instantiate (PrefabManager._instance._BossPrefab [_checkPointId]).GetComponent<BossController> ();
	}

}