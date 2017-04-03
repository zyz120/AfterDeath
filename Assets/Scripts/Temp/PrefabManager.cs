using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

	public static PrefabManager _instance;

	public GameObject[] _BossPrefab;
	public GameObject _hero;

	// Use this for initialization
	void Start () {
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
